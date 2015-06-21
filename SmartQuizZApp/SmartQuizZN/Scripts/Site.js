$(document).ready(function () {
    $(".pager").click(function (evt) {
        var pageindex = evt.target.innerHTML;
        $("#CurrentPageIndex").val(pageindex);
        evt.preventDefault();
        $("form").submit();
    });

    $(".start").click(function (evt) {
        var elem = document.getElementById("test").value;
        evt.preventDefault();
        $("form").submit();
    });


    var topics = document.getElementsByClassName("topics");
    $('#TopicName').change(function () {
        var length = topics.length;
        var userTopic = document.getElementById("TopicName").value;
        for(var i=0; i < length; i++)
        {
            if (topics[i].defaultValue == userTopic)
            {
                $(".topicName").append('<span id="TopicName-error" class="">This topic already exists in the database.</span>');
            }
        }

        if(userTopic == "")
        {
            $(".topicName").remove();
        }
    });

    $('form input').change(function () {
        $('form p').text(this.files.length + " file selected");
    });
});

   