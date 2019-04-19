$(document).ready(function () {

    var calendarEl = document.getElementById('user_calendar');

    var calendar = new FullCalendar.Calendar(calendarEl, {
        plugins: ['dayGrid', 'bootstrap'],
        themeSystem: 'bootstrap',
        themeName: 'lux' //fix
    });
    calendar.render();

    let sort_dir = window.sessionStorage.getItem("sort_direction");
    if (sort_dir === null) {
        sort_dir = "ascending";
    }

    let sort_option = window.sessionStorage.getItem("sort_option");
    if (sort_option === null) {
        sort_option = "new";
    }

    setSortDirection(sort_dir);

    setSortOption(sort_option);


    updateSort();

    $("#sortDirectionDropdown").on("click", function (e) {
        let button = $("#sortDirectionDropdown > i");
        if (button.hasClass("fa-arrow-up")) {
            setSortDirection("ascending");
        }
        else if (button.hasClass("fa-arrow-down")) {
            setSortDirection("descending");
        }

    });

    $("#sortDirectionDropdown").on("click", function (e) {
        let button = $("#sortDirectionDropdown > i");
        if (button.hasClass("fa-arrow-up")) {
            setSortDirection("descending");
        }
        else if (button.hasClass("fa-arrow-down")) {
            setSortDirection("ascending");
        }
        updateSort();
    });

    $("#sortByNew").on("click", function (e) {

        setSortOption("new");
        updateSort();

    });

    $("#sortByRating").on("click", function (e) {

        setSortOption("top");
        updateSort();

    });

});

function sort_by_rating(a, b) {

    let a_rating = parseInt($(a).data("rating"));
    let b_rating = parseInt($(b).data("rating"));

    if (a_rating > b_rating) return 1;
    if (a_rating < b_rating) return -1;
    return 0;
}

function sort_by_date(a, b) {
    let a_date = Date.parse($(a).data("date"));
    let b_date = Date.parse($(b).data("date"));

    if (a_date > b_date) return -1;
    if (a_date < b_date) return 1;
    return 0;
}

function sort_by_name(a, b) {
    return a.innerHTML.toLowerCase().localeCompare(b.innerHTML.toLowerCase());
}

function setSortOption(new_option) {

    window.sessionStorage.setItem("sort_option", new_option.toLowerCase());
    $("#sort_option_text").text(new_option.toLowerCase());

}

function setSortDirection(new_dir) {

    window.sessionStorage.setItem("sort_direction", new_dir);

    if (new_dir === "descending") {
        $("#sortDirectionDropdown > i").addClass("fa-arrow-up");
        $("#sortDirectionDropdown > i").removeClass("fa-arrow-down");
    }
    if (new_dir === "ascending") {

        $("#sortDirectionDropdown > i").addClass("fa-arrow-down");
        $("#sortDirectionDropdown > i").removeClass("fa-arrow-up");
    }


}

function updateSort() {
    let post_list = $(".post").get();

    let option = window.sessionStorage.getItem("sort_option");
    let order = window.sessionStorage.getItem("sort_direction");

    let compareFunction = sort_by_date;
    if (option.toLowerCase() === "top") compareFunction = sort_by_rating;


    post_list.sort(compareFunction);
    if (order === "ascending") post_list.reverse();

    for (let i = 0; i < post_list.length; i++) {
        post_list[i].parentNode.append(post_list[i]);
    }
}
