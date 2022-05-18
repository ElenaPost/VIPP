$(function () {
    var feedbackHub = $.connection.feedbackHub;
    
    feedbackHub.client.addFeedback = function (text) {
        debugger;
        $("#Feedback").empty();
        $("#Feedback").append("<h4>Комментарий консультанта</h4><p>" + text + "</p>");
    };

    feedbackHub.client.addFinalFeedback = function (text) {
        $("#FinalFeedback").empty();
        $("#FinalFeedback").append("<h4>Финальный комментарий консультанта</h4><p>" + text + "</p>");
    };

    $.connection.hub.start().done(function () {
        $("#Send").click(function () {
            var feedback = $(".feedback");
            feedbackHub.server.addFeedback(feedback.val(), feedback.attr("id"));
        });

        $("#FinalSend").click(function () {
            debugger
            var finalFeedback = $(".finalFeedback");
            feedbackHub.server.addFinalFeedback(finalFeedback.val(), finalFeedback.attr("id"));
        });
    });
});