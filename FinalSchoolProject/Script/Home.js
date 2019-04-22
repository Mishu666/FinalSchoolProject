$(document).ready(function () {


    var calendarEl = document.getElementById('user_calendar');

    var calendar = new FullCalendar.Calendar(calendarEl, {
        plugins: ['dayGrid', 'bootstrap'],
        themeSystem: 'bootstrap'
    });
    calendar.render();

});