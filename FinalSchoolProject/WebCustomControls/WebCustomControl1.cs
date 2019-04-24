using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace WebCustomControls
{
	public delegate void NestedRepeaterItemDataBoundHandler(object sender,
	NestedRepeaterItemEventArgs e);
	public delegate void NestedRepeaterItemCreatedHandler(object sender,
	NestedRepeaterItemEventArgs e );

	/// <summary>
	/// A Repeater designed to work with hierarchical data.
	/// </summary>
    [ParseChildren(ChildrenAsProperties = true),
    ToolboxData("<{0}:NestedRepeater runat=server></{0}:NestedRepeater>")]
    public class NestedRepeater : System.Web.UI.WebControls.WebControl,
        INamingContainer
    {
        #region Member Data
        private ITemplate m_itemTemplate, m_headerTemplate, m_footerTemplate;
        private DataSet m_dataSource;

        /// <summary>
        /// The filter that will identify which DataRow will be the top row
        /// </summary>
        private string m_rowFilterTop = String.Empty;

        /// <summary>
        /// Name of the Parent-Child DataRelation for the recursive data.
        /// </summary>
        private string m_relationName = String.Empty;

        /// <summary>
        /// Name of the Datamember inside the DataSource
        /// </summary>
        private string m_dataMember = String.Empty;

        /// <summary>
        /// Holds the number of children for each node.
        /// </summary>
        private ArrayList m_lstNbChildren;

        /// <summary>
        /// Holds the index for the current node in m_lstNbChildren 
        /// as this list is populated or read.
        /// </summary>
        private int m_current;

        // An array with all topmost items
        private NestedRepeaterItem[] m_items;

        #endregion

        #region Events
        /// <summary>
        /// If specified, an event handler for the event ItemDataBound
        /// </summary>
        public event NestedRepeaterItemDataBoundHandler NestedRepeaterItemDataBound;
        protected virtual void OnItemDataBound(NestedRepeaterItemEventArgs e)
        {
            if (NestedRepeaterItemDataBound != null)
                NestedRepeaterItemDataBound(this, e);
        }

        /// <summary>
        /// If specified, an event handler for the event ItemCreated.
        /// </summary>
        public event NestedRepeaterItemCreatedHandler NestedRepeaterItemCreated;
        protected void OnItemCreated(NestedRepeaterItemEventArgs e)
        {
            if (NestedRepeaterItemCreated != null)
            {
                NestedRepeaterItemCreated(this, e);
            }
        }

        #endregion

        #region Properties : datasource
        public virtual DataSet DataSource
        {
            get { return m_dataSource; }
            set { m_dataSource = value; }
        }

        public virtual string RowFilterTop
        {
            get { return m_rowFilterTop; }
            set { m_rowFilterTop = value; }
        }


        public virtual string DataMember
        {
            get { return m_dataMember; }
            set { m_dataMember = value; }
        }

        public string RelationName
        {
            get { return m_relationName; }
            set { m_relationName = value; }
        }

        #endregion

        #region Interface
        [TemplateContainer(typeof(NestedRepeaterItem))]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual ITemplate ItemTemplate
        {
            get { return m_itemTemplate; }
            set { m_itemTemplate = value; }
        }

        [TemplateContainer(typeof(NestedRepeaterHeaderFooter))]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual ITemplate HeaderTemplate
        {
            get { return m_headerTemplate; }
            set { m_headerTemplate = value; }
        }

        [TemplateContainer(typeof(NestedRepeaterHeaderFooter))]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual ITemplate FooterTemplate
        {
            get { return m_footerTemplate; }
            set { m_footerTemplate = value; }
        }

        #endregion

        public override void DataBind()
        {
            base.OnDataBinding(EventArgs.Empty);

            Controls.Clear();

            ClearChildViewState();

            if (!IsTrackingViewState)
                TrackViewState();

            CreateControlHierarchy(true);
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();

            CreateControlHierarchy(false);
        }

        /* This function actually builds the various child controls : header/footer templates
         * + a NestedRepeaterItem for each element in the data source.*/
        protected virtual void CreateControlHierarchy(bool createFromDataSource)
        {
            int nbTopNodes = 0;
            DataView dv = null;

            // HeaderTemplate
            if (m_headerTemplate != null)
            {
                NestedRepeaterHeaderFooter header = new NestedRepeaterHeaderFooter();
                m_headerTemplate.InstantiateIn(header);

                Controls.Add(header);

                if (createFromDataSource)
                    header.DataBind();
            }

            // ItemTemplate
            if (createFromDataSource)
            {
                if (DataSource != null &&
                    DataSource.Tables.Count != 0)
                {
                    DataTable tbSource;

                    if (DataMember != String.Empty)
                        tbSource = DataSource.Tables[DataMember];
                    else
                        tbSource = DataSource.Tables[0];

                    if (tbSource == null)
                        throw new ApplicationException("No valid DataTable in the specified position.");

                    /* When creating from the ViewState (on PostBack), we'll need to know how many nodes
                        * there are under each node. So, when creating from the datasource, we store this 
                        * information in m_lstNbChildren, which we'll also save in the viewstate.
                        * */
                    m_lstNbChildren = new ArrayList(tbSource.Rows.Count);

                    dv = new DataView(tbSource);

                    if (m_rowFilterTop != String.Empty)
                        dv.RowFilter = m_rowFilterTop;

                    nbTopNodes = dv.Count;
                    m_lstNbChildren.Add(nbTopNodes);
                }
            }
            else // PostBack : data will be retrieve from ViewState
            {
                m_lstNbChildren = (ArrayList)ViewState["ListNbChildren"];
                m_current = 0;

                if (m_lstNbChildren != null)
                    nbTopNodes = (int)m_lstNbChildren[m_current++];
                else
                    nbTopNodes = 0;
            }

            NestedElementPosition currentPos;

            m_items = new NestedRepeaterItem[nbTopNodes];

            for (int i = 0; i < nbTopNodes; ++i)
            {
                if (i == 0 && i == nbTopNodes - 1)
                    currentPos = NestedElementPosition.OnlyOne;
                else if (i == 0)
                    currentPos = NestedElementPosition.First;
                else if (i == nbTopNodes - 1)
                    currentPos = NestedElementPosition.Last;
                else
                    currentPos = NestedElementPosition.NULL;

                NestedRepeaterItem childItem;
                if (createFromDataSource)
                    childItem = CreateItem(dv[i].Row, 0, currentPos);
                else
                    childItem = CreateItem(null, 0, currentPos++);

                m_items[i] = childItem;
            }

            if (createFromDataSource)
                ViewState["ListNbChildren"] = m_lstNbChildren;

            // FooterTemplate
            if (m_footerTemplate != null)
            {
                NestedRepeaterHeaderFooter footer = new NestedRepeaterHeaderFooter();
                m_footerTemplate.InstantiateIn(footer);

                Controls.Add(footer);

                if (createFromDataSource)
                    footer.DataBind();
            }

            ChildControlsCreated = true;
        }

        private NestedRepeaterItem CreateItem(DataRow row, int depth, NestedElementPosition pos)
        {
            DataRow[] childRows;
            int nbChildren = 0;

            NestedRepeaterItem item = new NestedRepeaterItem();

            if (row != null) // we are reading data from the DataSource
            {
                // prepare data for the ViewState
                childRows = row.GetChildRows(RelationName);
                nbChildren = childRows.Length;
                m_lstNbChildren.Add(nbChildren);

                // set various data for the item
                item.Position = pos;
                item.NbChildren = childRows.Length;
                item.Depth = depth;
            }
            else // we are reading data from the viewstate
            {
                nbChildren = (int)
                    m_lstNbChildren[m_current++];
                childRows = new DataRow[nbChildren];
            }

            if (m_itemTemplate != null)
                m_itemTemplate.InstantiateIn(item);

            Controls.Add(item);

            NestedRepeaterItemEventArgs args =
                new NestedRepeaterItemEventArgs();

            args.Item = item;
            OnItemCreated(args);

            if (row != null)
            {
                item.DataItem = row;
                item.DataBind();
                OnItemDataBound(args);
            }

            // Recursive call
            NestedElementPosition currentPos;
            item.Items = new NestedRepeaterItem[nbChildren];

            for (int i = 0; i < nbChildren; ++i)
            {
                if (i == 0 && i == nbChildren - 1)
                    currentPos = NestedElementPosition.OnlyOne;
                else if (i == 0)
                    currentPos = NestedElementPosition.First;
                else if (i == nbChildren - 1)
                    currentPos = NestedElementPosition.Last;
                else
                    currentPos = NestedElementPosition.NULL;

                NestedRepeaterItem childItem;
                if (row != null)
                    childItem = CreateItem(childRows[i], depth + 1, currentPos);
                else
                    childItem = CreateItem(null, depth + 1, currentPos);

                item.Items[i] = childItem;
            }

            return item;
        }

        // the topmost items.
        public NestedRepeaterItem[] Items
        {
            get
            {
                EnsureChildControls();
                return m_items;
            }
        }
    }

	#region ItemTemplate
	public enum NestedElementPosition
	{
		First, // current record is the first child of the immediate parent
		Last, // current record is the last child of the immediate parent
		OnlyOne, // current record is the only child of the immediate parent
		NULL // None of the above
	}

	public class NestedRepeaterItem : Control, INamingContainer
	{
		private object m_dataItem;
		private int m_depth;
		private NestedElementPosition m_position;
		private int m_nbChildren;

		public NestedRepeaterItem()
		{
		}

        /// <summary>
        /// Represents the underlying data
        /// </summary>
		public virtual object DataItem
		{
			get{return m_dataItem;}
			set{m_dataItem = value;}
		}

		public virtual int Depth
		{
			get {return m_depth;}
			set {m_depth = value;}
		}

        /// <summary>
        /// Situation among its siblings
        /// </summary>
        /// <remarks>
        /// If we want a special display for the first or the last child of a node, 
        /// we check this property.
        /// </remarks>      
		public NestedElementPosition Position
		{
			get {return m_position;}
			set {m_position = value;}
		}

        /// <summary>
        /// Number of sub-nodes
        /// </summary>
		public int NbChildren
		{
			get {return m_nbChildren;}
			set {m_nbChildren = value;}
		}

        // An array with all the immediate children
        NestedRepeaterItem[] m_items;
        public NestedRepeaterItem[] Items
        {
            get { return m_items; }
            set { m_items = value; }
        }
	}

	public class NestedRepeaterItemEventArgs : EventArgs
	{
		private NestedRepeaterItem m_item;

		public NestedRepeaterItem Item
		{
			get {return m_item;}
			set {m_item = value;}
		}
	}
	#endregion

	#region Header-Footer Template
	public class NestedRepeaterHeaderFooter : Control, INamingContainer
	{
		private object m_dataItem;

		public virtual object DataItem
		{
			get{return m_dataItem;}
			set{m_dataItem = value;}
		}
	}
	#endregion
}
