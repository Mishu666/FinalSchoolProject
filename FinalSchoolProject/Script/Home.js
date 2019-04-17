$(document).ready(function () {

    var calendarEl = document.getElementById('user_calendar');

    var calendar = new FullCalendar.Calendar(calendarEl, {
        plugins: ['dayGrid', 'bootstrap'],
        themeSystem: 'bootstrap',
    });
    calendar.render();

    userLoggedIn(userLoggedInSuccessCallback);

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

function userLoggedInSuccessCallback(data) {

    if (data.d === true) {

        $(".upvote").on("click", function (e) {

            e.preventDefault();
            var post_id = $(this).data("post-id");
            var upvote_btn = $(this);
            var downvote_btn = $(".downvote[data-post-id='" + upvote_btn.data("post-id") + "']");
            var upvote_counter = $("#upvote_counter[data-post-id='" + post_id + "']");
            var downvote_counter = $("#downvote_counter[data-post-id='" + post_id + "']");

            if (upvote_btn.hasClass("active")) {
                upvote_btn.removeClass("active");
                upvote_counter.text(parseInt(upvote_counter.text()) - 1);
            }
            else {
                if (downvote_btn.hasClass("active")) {
                    downvote_btn.removeClass("active");
                    upvote_btn.addClass("active");
                    upvote_counter.text(parseInt(upvote_counter.text()) + 1);
                    downvote_counter.text(parseInt(downvote_counter.text()) - 1);

                }
                else {
                    upvote_btn.addClass("active");
                    upvote_counter.text(parseInt(upvote_counter.text()) + 1);
                }
            }

            Upvote(post_id, upvoteSuccess);

        });

        $(".downvote").on("click", function (e) {

            e.preventDefault();
            var post_id = $(this).data("post-id");
            var downvote_btn = $(this);
            var upvote_btn = $(".upvote[data-post-id='" + downvote_btn.data("post-id") + "']");
            var downvote_counter = $("#downvote_counter[data-post-id='" + post_id + "']");
            var upvote_counter = $("#upvote_counter[data-post-id='" + post_id + "']");

            if (downvote_btn.hasClass("active")) {
                downvote_btn.removeClass("active");
                downvote_counter.text(parseInt(downvote_counter.text()) - 1);
            }
            else {
                if (upvote_btn.hasClass("active")) {
                    upvote_btn.removeClass("active");
                    downvote_btn.addClass("active");
                    downvote_counter.text(parseInt(downvote_counter.text()) + 1);
                    upvote_counter.text(parseInt(upvote_counter.text()) - 1);

                }
                else {
                    downvote_btn.addClass("active");
                    downvote_counter.text(parseInt(downvote_counter.text()) + 1);
                }
            }

            Downvote(post_id, downvoteSuccess);

        });
    }
    else {

        $('[data-toggle="popover"]').popover();

        $(".upvote").on("click", function (e) {
            console.log("oops");
            e.preventDefault();
            $(this).popover();

        });

        $(".downvote").on("click", function (e) {

            e.preventDefault();
            $(this).popover();

        });

    }
}

function downvoteSuccess(data) {
}

function upvoteSuccess(data) {
}

function Upvote(postID, success_callback) {

    var data = { "PostID": postID };

    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/Upvote",
        error: function (r) {
            console.log("error");
            console.log(r.status);
        },
        success: success_callback

    });

}

function Downvote(postID, success_callback) {

    var data = { "PostID": postID };

    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/Downvote",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback

    });

}