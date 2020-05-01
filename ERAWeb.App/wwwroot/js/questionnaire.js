var tabContents = $("div#myTab1Content").find("div.tab-pane");
var tabs = $("ul#myTab1").find("a.nav-link");
var buttonsDiv = $("div.card-footer").find("div.row");
var navLinks = $("ul#myTab1").find("a.nav-link");

$(document).on("blur", "div#bmiVariables input[type='number']", function () {
    var weight = $("div#bmiVariables input[type='number']").filter("input[id*='Weight']").val();
    var height = $("div#bmiVariables input[type='number']").filter("input[id*='Height']").val();
    var bmiResult = $("div#bmiVariables").find("a#bmiResult");
    var resultClass = "btn-success";
    if (weight != "" && height != "") {
        var bmi = weight / (height * height);
        if (!isNaN(bmi)) {
            if (bmi < 18.5) { resultClass = "btn-danger"; }
            if (bmi >= 18.5 && bmi <= 24.9) { resultClass = "btn-success"; }
            if (bmi >= 25 && bmi <= 30) { resultClass = "btn-warning"; }
            if (bmi > 30) { resultClass = "btn-danger"; }
        }
        bmiResult.html("BMI Score: " + bmi).attr("class", "").addClass("btn btn-pill btn-block " + resultClass);
    }
    else {
        bmiResult.html("").attr("class", "");
    }
    hideErrorOnChange($(this));
});

$(document).on("change", "div#workStation input[type='checkbox']", function () {
    if ($(this).attr("value").indexOf("Sitting") >= 0) {
        if ($(this).is(":checked")) {
            $("a#5").show().html("<i class='fas fa-chair'></i>&nbsp;&nbsp;Sitting Position");
            $("a#6").show().html("<i class='fas fa-desktop'></i>&nbsp;&nbsp;Computer Position When Sitting");
            $("div#divSittingPosition").removeClass("inactive").addClass("active");
            $("div#divStandingPosition").removeClass("active").addClass("inactive");
            $("div#divComputerPositionWhenSitting").removeClass("inactive").addClass("active");
            $("div#divComputerPositionWhenStanding").removeClass("active").addClass("inactive");
            $("div.card-footer").find("div#4").find("button#btnNext").show();
        }
    }
    else if ($(this).attr("value").indexOf("Standing") >= 0) {
        if ($(this).is(":checked")) {
            $("a#5").show().html("<i class='fas fa-child'></i>&nbsp;&nbsp;Standing Position");
            $("a#6").show().html("<i class='fas fa-desktop'></i>&nbsp;&nbsp;Computer Position When Standing");
            $("div#divSittingPosition").removeClass("active").addClass("inactive");
            $("div#divStandingPosition").removeClass("inactive").addClass("active");;
            $("div#divComputerPositionWhenSitting").removeClass("active").addClass("inactive");
            $("div#divComputerPositionWhenStanding").removeClass("inactive").addClass("active");;
            $("div.card-footer").find("div#4").find("button#btnNext").show();
        }
    }
    if (!$("div#workStation input[type='checkbox']").is(":checked")) {
        $("a#5").hide().html("Position");
        $("a#6").hide().html("Computer Position");
        $("div.card-footer").find("div#4").find("button#btnNext").hide();
        $("div#divSittingPosition").removeClass("active").addClass("inactive");
        $("div#divStandingPosition").removeClass("active").addClass("inactive");
        $("div#divComputerPositionWhenSitting").removeClass("active").addClass("inactive");
        $("div#divComputerPositionWhenStanding").removeClass("active").addClass("inactive");
    }
});

$(document).ready(function () {
    bindPopOver();
    checkAlternativeCheckBox();
    bindNextPrevButtonEvents();
    setNavigationButtonsByTabSelected();
    bindSubmitValidation();
    bindPopoverInfo();
});

var setNavigationButtonsByTabSelected = function () {
    $(".nav-tabs a[data-toggle=tab]").on("click", function (e) {
        var activeTabID = tabs.filter("a.nav-link.active").attr("id");
        var clickedTabID = $(this).attr("id");
        if (clickedTabID > activeTabID) {
            if (!validateTabControls(tabContents.filter("div.tab-pane.fade.active.show").find("div.active"))) {
                e.preventDefault();
                return false;
            }
            else {
                setTabContent($(this));
            }
        }
        else {
            setTabContent($(this));
        }
    });
}

var bindNextPrevButtonEvents = function () {
    $("button#btnNext").click(function () {
        if (validateTabControls(tabContents.filter("div.tab-pane.fade.active.show").find("div.active"))) {
            showActiveTab(true);
        }
    });

    $("button#btnPrev").click(function () {
        showActiveTab(false);
    });
}

var validateTabControls = function (activeTabContent) {
    var result = true;
    var controlGroup = activeTabContent.find("div.controlGroup");
    controlGroup.each(function () {
        if ($(this).find("input[type='checkbox']").length > 0) {
            if (!$(this).find("input[type='checkbox']").is(":checked")) {
                //$(this).parents("div.card-body").addClass("errorBorder");
                $(this).parents("div.card").eq(0).find("div.card-footer").addClass("errorBorder").show();
                result = false;
            }
            else {
                $(this).parents("div.card").eq(0).find("div.card-footer").removeClass("errorBorder").hide();
            }
        }
        if ($(this).find("input[type='number']").length > 0) {
            if ($(this).find("input[type='number']").val() == "") {
                $(this).parents("div.card").eq(0).find("div.card-footer").addClass("errorBorder").show();
                result = false;
            }
            else {
                $(this).parents("div.card").eq(0).find("div.card-footer").removeClass("errorBorder").hide();
            }
        }
    });
    return result;
}

var hideErrorOnChange = function (control) {
    control.parents("div.card").eq(0).find("div.card-footer").removeClass("errorBorder").hide();
}

var checkAlternativeCheckBox = function () {
    $("div.controlGroup").find("input[type='checkbox']").change(function () {
        if ($(this).is(":checked")) {
            $(this).prop("checked", "checked");
            $(this).parents("div.controlGroup").find("input[type='checkbox']").not($(this)).prop("checked", false);
            hideErrorOnChange($(this));
        }
        bindTabSummary();
    });
}

var bindPopOver = function () {
    $('[data-toggle="popover"]').popover({ 'placement': 'top', trigger: 'hover' });
}

var setTabContent = function (tab) {
    var tabID = tab.attr("id");
    buttonsDiv.hide();
    var nextButtonDiv = buttonsDiv.filter("div#" + tabID);
    nextButtonDiv.show();
}


var showActiveTab = function (isNext) {
    var activeDiv = tabContents.filter("div.tab-pane.fade.active.show");
    var activeTab = tabs.filter("a.nav-link.active");
    var index = tabContents.index(activeDiv);
    var nextDiv = null;
    var nextTab = null;
    var nextBut = null;
    var nextIndex = 0;
    if (isNext) { nextIndex = index + 1; }
    else { nextIndex = index - 1; }

    nextDiv = tabContents.eq(nextIndex);
    nextTab = tabs.eq(nextIndex);
    nextBut = buttonsDiv.eq(nextIndex);

    activeDiv.removeClass("active").removeClass("show");
    activeTab.removeClass("active").attr("aria-selected", "false");
    buttonsDiv.hide();
    nextDiv.addClass("active").addClass("show");
    nextTab.addClass("active").attr("aria-selected", "true");
    nextBut.show();
}

var bindSubmitValidation = function () {
    $("button#btnSubmit").click(function () {
        if (validateTabControls(tabContents.filter("div.tab-pane.fade.active.show").find("div.active"))) {
            return true;
        }
        return false;
    });
}

var bindTabSummary = function () {
    var activeTab = tabContents.filter("div.tab-pane.fade.active.show").find("div.active");
    var lowRisk = 0, highRisk = 0, moderateRisk = 0;
    var control = activeTab.find("label.switch-pill");
    control.each(function () {
        if ($(this).find("input[type='checkbox']").is(":checked")) {
            lowRisk = lowRisk + ($(this).hasClass("switch-success") == true ? 1 : 0);
            moderateRisk = moderateRisk + ($(this).hasClass("switch-warning") == true ? 1 : 0);
            highRisk = highRisk + ($(this).hasClass("switch-danger") == true ? 1 : 0);
        }
    });
    showSummary(lowRisk, moderateRisk, highRisk);
}

var showSummary = function (lowRisk, moderateRisk, highRisk) {
    var activeTab = tabContents.filter("div.tab-pane.fade.active.show").find("div.active");
    var lowRiskBtn = activeTab.find("div#divSummary").find("a.btn-success");
    var moderateRiskBtn = activeTab.find("div#divSummary").find("a.btn-warning");
    var highRiskBtn = activeTab.find("div#divSummary").find("a.btn-danger");
    var totalBtn = activeTab.find("div#divSummary").find("a.btn-info");
    var averageBtn = activeTab.find("div#divSummary").find("a#btnResult");
    if (activeTab.find("div#divSummary").hasClass("humanVariables")) {
        lowRiskBtn.html(lowRiskBtn.attr("value") + " " + lowRisk);
        moderateRiskBtn.html(moderateRiskBtn.attr("value") + " " + moderateRisk);
        highRiskBtn.html(highRiskBtn.attr("value") + " " + highRisk);
        totalBtn.html(totalBtn.attr("value") + " " + (lowRisk + moderateRisk + highRisk));
    }

    if (activeTab.find("div#divSummary").hasClass("discomformts")) {
        var resultClass = "";
        var text = "";
        if (highRisk <= 1) { text = "Low Risk: "; resultClass = "btn btn-pill btn-block btn-success active"; }
        if (highRisk >= 2 && highRisk <= 3) { text = "Moderate Risk: "; resultClass = "btn btn-pill btn-block btn-warning active"; }
        if (highRisk >= 4) { text = "High Risk: "; resultClass = "btn btn-pill btn-block btn-danger active"; }
        averageBtn.html(text + " " + highRisk);
        averageBtn.attr("class", resultClass);
    }

    if (activeTab.find("div#divSummary").hasClass("average")) {
        var resultClass = "";
        var text = "";
        highRisk = highRisk / (highRisk + lowRisk);
        if (highRisk <= 0.3333333333333333) { text = "Low Risk: "; resultClass = "btn btn-pill btn-block btn-success active"; }
        if (highRisk >= 0.3333333333333334 && highRisk <= 0.6666666666666666) { text = "Moderate Risk: "; resultClass = "btn btn-pill btn-block btn-warning active"; }
        if (highRisk >= 0.6666666666666667) { text = "High Risk: "; resultClass = "btn btn-pill btn-block btn-danger active"; }
        averageBtn.html(text + " " + highRisk);
        averageBtn.attr("class", resultClass);
    }
}

var bindPopoverInfo = function () {
    $('a.infoImageRecoImage').popover({
        title: function () {
            var infoTitle = $(this).parent().find("input#hdnTitle").attr("value");
            return infoTitle + '<a class="close" href="#">&times;</a>';
        },
        placement: 'auto',
        trigger: 'click',
        html: true,
        content: function () {
            $(".popover").popover('hide'); //hide existing popover

            var divContent = $(this).parent();
            var infoTitle = divContent.find("input#hdnInformationTitle").attr("value");
            var info = divContent.find("input#hdnInformationText").attr("value");
            var infoImage = divContent.find("input#hdnInformationImageURL").attr("value");
            var infoImageText = divContent.find("input#hdnInformationImageText").attr("value");

            var recoTitle = divContent.find("input#hdnRecommendationTitle").attr("value");
            var reco = divContent.find("input#hdnRecommendationText").attr("value");
            var recoImage = divContent.find("input#hdnRecommendationImageURL").attr("value");
            var recoImageText = divContent.find("input#hdnRecommendationImageText").attr("value");

            var innerInfoDiv = '<div class="row"><div class="col-md-6"><h6 class="text-center">' + infoImageText + '</h6><img src="images/information/' + infoImage + '" ></div><div class="col-md-6"><p>' + info + '</p></div></div>';
            var innerRecoDiv = '<div class="row"><div class="col-md-6"><h6 class="text-center">' + recoImageText + '</h6><img src="images/information/' + recoImage + '" ></div><div class="col-md-6"><p>' + reco + '</p></div></div>';

            var infoCard = '<div class="card"><div class="card-header"><i class="fa fa-info"></i>&nbsp;' + infoTitle + '</div><div class="card-body">' + innerInfoDiv + '</div></div>';
            var recoCard = '<div class="card"><div class="card-header"><i class="fa fa-check"></i>&nbsp; ' + recoTitle + '</div><div class="card-body">' + innerRecoDiv + '</div></div>';

            return '<div class="card"><div class="card-body">' + infoCard + '</hr>' + recoCard + '</div></div>';
        }
    });

    $('a.infoImageReco').popover({
        title: function () {
            var infoTitle = $(this).parent().find("input#hdnTitle").attr("value");
            return infoTitle + '<a class="close" href="#">&times;</a>';
        },
        placement: 'auto',
        trigger: 'click',
        html: true,
        content: function () {
            $(".popover").popover('hide'); //hide existing popover

            var divContent = $(this).parent();
            var infoTitle = divContent.find("input#hdnInformationTitle").attr("value");
            var info = divContent.find("input#hdnInformationText").attr("value");
            var infoImage = divContent.find("input#hdnInformationImageURL").attr("value");
            var infoImageText = divContent.find("input#hdnInformationImageText").attr("value");

            var recoTitle = divContent.find("input#hdnRecommendationTitle").attr("value");
            var reco = divContent.find("input#hdnRecommendationText").attr("value");

            var leftContent = '<div class="float-left mr-5"><h6 class="text-center">' + infoImageText + '</h6><img src="images/information/' + infoImage + '" ></div>';

            var innerInfoDiv = '<div class="row"><div class="col-md-12"><p>' + info + '</p></div></div>';
            var innerRecoDiv = '<div class="row"><div class="col-md-12"><p>' + reco + '</p></div></div>';

            var infoCard = '<div class="card"><div class="card-header"><i class="fa fa-info"></i>&nbsp;' + infoTitle + '</div><div class="card-body">' + innerInfoDiv + '</div></div>';
            var recoCard = '<div class="card"><div class="card-header"><i class="fa fa-check"></i>&nbsp; ' + recoTitle + '</div><div class="card-body">' + innerRecoDiv + '</div></div>';
            var rightContent = '<div>' + infoCard + '</hr>' + recoCard + '</div>';
            return '<div class="card"><div class="card-body"><div class="container">' + leftContent + rightContent + '</div></div></div>';
        }
    });

    $('a.infoImage').popover({
        title: function () {
            var infoTitle = $(this).parent().find("input#hdnTitle").attr("value");
            return infoTitle + '<a class="close" href="#">&times;</a>';
        },
        placement: 'auto',
        trigger: 'click',
        html: true,
        content: function () {
            $(".popover").popover('hide'); //hide existing popover

            var divContent = $(this).parent();
            var infoTitle = divContent.find("input#hdnInformationTitle").attr("value");
            var info = divContent.find("input#hdnInformationText").attr("value");
            var infoImage = divContent.find("input#hdnInformationImageURL").attr("value");
            var infoImageText = divContent.find("input#hdnInformationImageText").attr("value");

            var innerInfoDiv = '<div class="row"><div class="col-md-6"><h6 class="text-center">' + infoImageText + '</h6><img src="images/information/' + infoImage + '" ></div><div class="col-md-6"><p>' + info + '</p></div></div>';

            var infoCard = '<div class="card"><div class="card-header"><i class="fa fa-info"></i>&nbsp;' + infoTitle + '</div><div class="card-body">' + innerInfoDiv + '</div></div>';

            return '<div class="card"><div class="card-body">' + infoCard + '</div></div>';
        }
    });

    $('a.biggerInfoImage').popover({
        title: function () {
            var infoTitle = $(this).parent().find("input#hdnTitle").attr("value");
            return infoTitle + '<a class="close" href="#">&times;</a>';
        },
        placement: 'auto',
        trigger: 'click',
        html: true,
        content: function () {
            $(".popover").popover('hide'); //hide existing popover

            var divContent = $(this).parent();
            var infoTitle = divContent.find("input#hdnInformationTitle").attr("value");
            var info = divContent.find("input#hdnInformationText").attr("value");
            var infoImage = divContent.find("input#hdnInformationImageURL").attr("value");
            var infoImageText = divContent.find("input#hdnInformationImageText").attr("value");

            var innerInfoDiv = '<div class="row"><div class="col-md-8"><h6 class="text-center">' + infoImageText + '</h6><img width="100%" src="images/information/' + infoImage + '" ></div><div class="col-md-4"><p>' + info + '</p></div></div>';

            var infoCard = '<div class="card"><div class="card-header"><i class="fa fa-info"></i>&nbsp;' + infoTitle + '</div><div class="card-body">' + innerInfoDiv + '</div></div>';

            return '<div class="card"><div class="card-body">' + infoCard + '</div></div>';
        }
    });

    $('a.imageOnly').popover({
        title: function () {
            var infoTitle = $(this).parent().find("input#hdnTitle").attr("value");
            return infoTitle + '<a class="close" href="#">&times;</a>';
        },
        placement: 'auto',
        trigger: 'click',
        html: true,
        content: function () {
            $(".popover").popover('hide'); //hide existing popover

            var divContent = $(this).parent();
            var infoTitle = divContent.find("input#hdnInformationTitle").attr("value");
            var infoImage = divContent.find("input#hdnInformationImageURL").attr("value");
            var infoImageText = divContent.find("input#hdnInformationImageText").attr("value");

            var innerInfoDiv = '<div class="row"><div class="col-md-12"><h6 class="text-center">' + infoImageText + '</h6><img src="images/information/' + infoImage + '" ></div></div>';

            var infoCard = '<div class="card"><div class="card-header"><i class="fa fa-info"></i>&nbsp;' + infoTitle + '</div><div class="card-body">' + innerInfoDiv + '</div></div>';

            return '<div class="card"><div class="card-body">' + infoCard + '</div></div>';
        }
    });

    $(document).on("click", ".popover .close", function () {
        $(this).parents(".popover").popover('hide');
        return false;
    });
}


var getPopOverContent = function (divContent) {
    debugger;
    var info = divContent.find("input#hdnInformationText").attr("value");
    var infoImage = divContent.find("input#hdnInformationImageURL").attr("value");
    var infoImageText = divContent.find("input#hdnInformationImageText").attr("value");
    var reco = divContent.find("input#hdnRecommendationText").attr("value");
    var recoImage = divContent.find("input#hdnRecommendationImageURL").attr("value");
    var recoImageText = divContent.find("input#hdnRecommendationImageText").attr("value");
    var innerInfoDiv = '<div class="row"><div class="col-md-6"><h2>' + infoImageText + '</h2><img src="images/information/' + infoImage + '" ></div><div class="col-md-6"><h2>' + infoImageText + '</h2><p>' + info + '</p></div></div>';
    var innerRecoDiv = '<div class="row"><div class="col-md-6"><h2>' + recoImageText + '</h2><img src="images/information/' + recoImage + '" ></div><div class="col-md-6"><h2>' + recoImageText + '</h2><p>' + reco + '</p></div></div>';
    return '<div class="card"><div class="card-body">' + innerInfoDiv + innerRecoDiv + '</div></div>';
}
