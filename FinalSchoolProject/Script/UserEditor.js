$(document).ready(function () {

    AssignAdminSwitchLabels();
    AssignSuspendedSwitchLabels();

});

function AssignAdminSwitchLabels() {

    let switches = $(".IsAdminSwitch");
    let switchLabel;
    let switchContainer;

    for (let s of switches) {

        switchContainer = $(s.closest("div"));
        switchLabel = switchContainer.find("label").first();

        switchLabel.attr("for", s.id);

    }
}

function AssignSuspendedSwitchLabels() {

    let switches = $(".IsSuspendedSwitch");
    let switchLabel;
    let switchContainer;

    for (let s of switches) {

        switchContainer = $(s.closest("div"));
        switchLabel = switchContainer.find("label").first();

        switchLabel.attr("for", s.id);

    }
}