(function (module) {
    "use strict";
    const _configs = module.configs,
        _root = '#common-cportallogin-wrapper';

    // Create root
    let _PChaseCommon = function () {
        this._companyCode = localStorage.getItem("hdnCompanyCode") === null ? sessionStorage.getItem("CPortalCompanyCode") === null ? '' : sessionStorage.getItem("CPortalCompanyCode") : localStorage.getItem("hdnCompanyCode");
        this._locationCode = localStorage.getItem("hdnLocationCode") === null ? sessionStorage.getItem("CPortalLocationCode") === null ? '' : sessionStorage.getItem("CPortalLocationCode") : localStorage.getItem("hdnLocationCode");
        this._companyRegion = 0;
        this._companyNumberOfDecimal = 1;
        this._companyAppFilePath = '';
        this._locationSiteId = '';
        this._locationDateFormats = '';
        this._locationWeekStart = '';
        this._locationMonthStart = '';
        this._locationNetAmout = '';
        this._locationCurrency = '';
        this._locationUKDashboardSiteId = '';
        this._GetCurrentProjectBaseUrl = '';
        this._SelectedModuleCode = '';
        this._pageIndex = 0;
        this._pageSize = 25;
        this._searchData = [];
        this._erpName = '';
        this._enableOCR = false;
    };

    // Page Load After Call This Function
    _PChaseCommon.prototype.init = function () {
        module._PChaseCommon.load();
        module._PChaseCommon.initValidation();
    }

    _PChaseCommon.prototype.load = function () {
        if (PChaseModule.configs.loginUserType === 'vendorexternal') {
            PChaseModule._PChaseCommon.ChangeEventForddlLocationList();
        } else {
            if ($("#hdnUnderMaintenance").val() === 'No') {
                if ($("#hdnIsGrouping").val() === 'Yes') {
                    if (PChaseModule._PChaseCommon._companyCode === '') {
                        var $group = $(".group-checkbox:eq(0)").closest('.group');
                        $group.find(".group-checkbox").prop('checked', true).change();
                        $group.find(".company-checkbox:first").prop('checked', true).change();
                    } else {
                        $.each(PChaseModule._PChaseCommon._companyCode.split(","), function (i, e) {
                            var $group = $(`${_root} .company-checkbox[data-ccode=` + e + `]`).closest('.group');
                            $group.find(`.company-checkbox[data-ccode=` + e + `]`).prop('checked', true);
                            if ($group.find(`.company-checkbox`).length === $group.find(`.company-checkbox:checked`).length) {
                                $group.find(".group-checkbox").prop('checked', true).change();
                            }
                        });
                        $(`${_root} .company-checkbox:checked`).first().change();
                    }
                } else {
                    PChaseModule._PChaseSelect2.SingleSelect2("ddlCompanyList", "id", "Company");
                }
                PChaseModule._PChaseSelect2.MultipleSelect2("ddlLocationList", "id", "Location");
                if (PChaseModule._PChaseCommon._companyCode === '') {
                    $(`${_root} #ddlCompanyList`).val($(`${_root} #ddlCompanyList option:eq(1)`).val()).trigger('change');
                } else {
                    var isContained = PChaseModule._PChaseCommon._companyCode.indexOf(',') > -1;
                    if (isContained) {
                        var str = PChaseModule._PChaseCommon._companyCode;
                        var arr = str.split(",");
                        var fst = arr.splice(0, 1).join("");
                        PChaseModule._PChaseCommon._companyCode = fst;
                    }
                    $(`${_root} #ddlCompanyList`).val(PChaseModule._PChaseCommon._companyCode).trigger('change');
                }
                PChaseModule._PChaseCommon.HideShowReviewNotification();
            } else {
                $("#layout_company_list_dropdown").hide();
                $("#layout_location_list_drop").hide();
            }
        }
    }
    _PChaseCommon.prototype.initValidation = function () {
        
        var regex = /^[a-zA-Z0-9\s]+$/;

        $(document).on("keypress", "input[data-val='nospecial']", function (e) {
            var key = String.fromCharCode(e.which);

            if (!/^[a-zA-Z0-9\s]$/.test(key)) {
                e.preventDefault();
                showError(this, "Special characters not allowed");
                return false;
            } else {
                clearError(this);
            }
        });

        $(document).on("blur", "input[data-val='nospecial']", function () {
            var value = $(this).val();

            if (value && !regex.test(value)) {
                showError(this, "Invalid characters detected");
                $(this).val("");
            } else {
                clearError(this);
            }
        });

        function showError(el, msg) {
            var id = $(el).attr("id") + "_err";

            if ($("#" + id).length === 0) {
                $(el).after("<span class='text-danger' id='" + id + "'>" + msg + "</span>");
            } else {
                $("#" + id).text(msg);
            }
        }

        function clearError(el) {
            var id = $(el).attr("id") + "_err";
            $("#" + id).text("");
        }
    }
    _PChaseCommon.prototype.HideShowReviewNotification = function () {
        if (parseInt($(`${_root} #taskManagerReviewCount`).attr('data-rcount')) <= 0) {
            $('.bell_notify_block').hide();
        }
        else {
            $('.bell_notify_block').show();
        }
        if (parseInt($(`${_root} #taskManagerReviewCount`).attr('data-frscount')) <= 0) {
            $('.notification_content3').hide();
        }
        else {
            $('.notification_content3').show();
        }
        if (parseInt($(`${_root} #taskManagerReviewCount`).attr('data-fracount')) <= 0) {
            $('.notification_content4').hide();
        }
        else {
            $('.notification_content4').show();
        }
        if (parseInt($(`${_root} #taskManagerReviewCount`).attr('data-srscount')) <= 0) {
            $('.notification_content5').hide();
        }
        else {
            $('.notification_content5').show();
        }
    }

    _PChaseCommon.prototype.updateVariable = function () {
        if ($("#hdnIsGrouping").val() === 'Yes') {
            this._companyCode = $(".company-checkbox:checked").map(function (i, v) {
                return $(this).attr('data-ccode');
            }).get().join(',');
            localStorage.setItem("hdnCompanyCode", this._companyCode);
            sessionStorage.setItem("CPortalCompanyCode", this._companyCode);
        } else {
            localStorage.setItem("hdnCompanyCode", $(`${_root} select.ddlCompanyList option:selected`).val());
            sessionStorage.setItem("CPortalCompanyCode", $(`${_root} select.ddlCompanyList option:selected`).val());
            this._companyCode = $(`${_root} select.ddlCompanyList option:selected`).val();
        }
        var Location_all = $(`${_root} .ddlLocationList option:selected`).map(function () {
            return $(this).val();
        }).get().join(',');
        localStorage.setItem("hdnLocationCode", Location_all);
        sessionStorage.setItem("CPortalLocationCode", Location_all);
        this._locationCode = Location_all;
        this._companyRegion = $(`${_root} select.ddlCompanyList option:selected`).attr('data-region');
        this._companyNumberOfDecimal = $(`${_root} select.ddlCompanyList option:selected`).attr('data-numberofdecimal');
        this._companyAppFilePath = $(`${_root} select.ddlCompanyList option:selected`).attr('data-appfilepath');
        this._locationSiteId = $(`${_root} select.ddlLocationList option:selected`).attr('data-uksiteid');
        this._locationDateFormats = $(`${_root} select.ddlLocationList option:selected`).attr('data-ukdateformats');
        this._locationWeekStart = $(`${_root} select.ddlLocationList option:selected`).attr('data-ukweekstart');
        this._locationMonthStart = $(`${_root} select.ddlLocationList option:selected`).attr('data-ukmonthstart');
        this._locationNetAmout = $(`${_root} select.ddlLocationList option:selected`).attr('data-ukisnetamout');
        this._locationCurrency = $(`${_root} select.ddlLocationList option:selected`).attr('data-ukcurrency');
        this._locationUKDashboardSiteId = $(`${_root} select.ddlLocationList option:selected`).attr('data-ukdashboardsiteid');
        this._erpName = $(`${_root} select.ddlLocationList option:selected`).attr('data-erpname');
        this._enableOCR = $(`${_root} select.ddlLocationList option:selected`).attr('data-enableocr');
    }

    _PChaseCommon.prototype.ChangeEventForddlCompanyList = function ($this) {
        $(`${_root} #ddlLocationList`).find('option').remove();
        if ($("#hdnIsGrouping").val() === 'Yes') {
            let _companyGroupNSt = true;
            var _companyCode = $(".company-checkbox:checked").map(function (i, v) {
                return $(this).attr('data-ccode');
            }).get().join(',');
            var _companyGroupN = $(".company-checkbox:checked").map(function (i, v) {
                return $(this).attr('data-gname');
            }).get().join(',');
            var whateverValue = $($this).attr('data-gname');
            _companyGroupN.split(',').forEach(function (host) {
                if (whateverValue.indexOf(host) === -1) {
                    PChaseModule._PChanseToaster._toastr(0, "Please select companies from one group only.");
                    _companyGroupNSt = false;
                    var $group = $($this).closest('.group');
                    $group.find('.company-checkbox').prop('checked', false);
                }
            });
            if (_companyGroupNSt === true && _companyCode !== '') {
                let url = module.urls.bindLocationList;
                let data = { 'CompanyCode': _companyCode };//is null '{}'
                PChaseModule._PChaseAjaxResponse.GetAjaxJsonResponse(url, data, function (response) {
                    PChaseModule._PChaseCommon._locationCode = '';
                    $(`${_root} #ddlLocationList`).find('option').remove();
                    $(`${_root} #ddlLocationList`).find('optgroup').remove();
                    if (response !== null && response !== undefined) {
                        const $dropdown = $(`${_root} #ddlLocationList`);
                        var groubedByTeam = [response.reduce((a, c) => (a[c.companyCode] = (a[c.companyCode] || []).concat(c), a), {})];
                        groubedByTeam.forEach(function (outerObj) {
                            Object.keys(outerObj).forEach(function (key) {
                                var $optgroup = $('<optgroup>').attr('label', outerObj[key][0].companyCode + ' - ' + outerObj[key][0].companyName);
                                outerObj[key].forEach(function (gitem) {
                                    $(gitem).each(function (index, item) {
                                        if (item.selected === true) {
                                            const $option = $('<option selected="selected" data-appfilepath="' + item.appFilePath + '" data-ukdashboardsiteid="' + item.ukdashboardSiteId + '" data-uksiteid="' + item.ukSiteId + '" data-ukdateformats="' + item.dateFormats + '" data-ukweekstart="' + item.weekStart + '" data-ukmonthstart="' + item.monthStart + '" data-ukisnetamout="' + item.isNetAmout + '" data-ukcurrency="' + item.currency + '" data-erpname="' + item.erpName + '" data-enableocr="' + item.enableOCR + '"></option>').val(item.value).html(item.text);
                                            $optgroup.append($option);
                                        } else {
                                            const $option = $('<option data-appfilepath="' + item.appFilePath + '" data-ukdashboardsiteid="' + item.ukdashboardSiteId + '" data-uksiteid="' + item.ukSiteId + '" data-ukdateformats="' + item.dateFormats + '" data-ukweekstart="' + item.weekStart + '" data-ukmonthstart="' + item.monthStart + '" data-ukisnetamout="' + item.isNetAmout + '" data-ukcurrency="' + item.currency + '" data-erpname="' + item.erpName + '" data-enableocr="' + item.enableOCR + '"></option>').val(item.value).html(item.text);
                                            $optgroup.append($option);
                                        }
                                    });
                                    $dropdown.append($optgroup);
                                });
                            });
                        });
                        var Location_all = $(`${_root} .ddlLocationList option:selected`).map(function () { return $(this).val(); }).get().join(',');
                        $(".ddlLocationList option").prop("selected", false);
                        if (PChaseModule._PChaseCommon._locationCode !== '' && Location_all.indexOf(PChaseModule._PChaseCommon._locationCode) !== -1) {
                            $.each(PChaseModule._PChaseCommon._locationCode.split(","), function (i, e) {
                                $(`${_root} .ddlLocationList option[value=` + e + `]`).prop("selected", true);
                            });
                            $(`${_root} #ddlLocationList`).trigger('change');
                        } else {
                            $.each(Location_all.split(","), function (i, e) {
                                $(`${_root} .ddlLocationList option[value=` + e + `]`).prop("selected", true);
                            });
                            $(`${_root} #ddlLocationList`).trigger('change');
                        }
                    }
                }, true, false);
            } else if (_companyGroupNSt === true && _companyCode === '') {
                $(`#ddlLocationList`).find('option').remove();
                $(`#ddlLocationList`).find('optgroup').remove();
            }
        } else {
            let url = module.urls.bindLocationList;
            let data = { 'CompanyCode': $($this).val() };//is null '{}'
            PChaseModule._PChaseAjaxResponse.GetAjaxJsonResponse(url, data, function (response) {
                $(`${_root} #ddlLocationList`).find('option').remove();
                $(response).each(function (index, item) { // GETTING ERROR HERE
                    if (item.selected === true) {
                        $(`${_root} #ddlLocationList`).append($('<option selected="selected" data-appfilepath="' + item.appFilePath + '" data-ukdashboardsiteid="' + item.ukdashboardSiteId + '" data-uksiteid="' + item.ukSiteId + '" data-ukdateformats="' + item.dateFormats + '" data-ukweekstart="' + item.weekStart + '" data-ukmonthstart="' + item.monthStart + '" data-ukisnetamout="' + item.isNetAmout + '" data-ukcurrency="' + item.currency + '" data-erpname="' + item.erpName + '" data-enableocr="' + item.enableOCR + '"></option>').val(item.value).html(item.text));
                    } else {
                        $(`${_root} #ddlLocationList`).append($('<option data-appfilepath="' + item.appFilePath + '" data-ukdashboardsiteid="' + item.ukdashboardSiteId + '" data-uksiteid="' + item.ukSiteId + '" data-ukdateformats="' + item.dateFormats + '" data-ukweekstart="' + item.weekStart + '" data-ukmonthstart="' + item.monthStart + '" data-ukisnetamout="' + item.isNetAmout + '" data-ukcurrency="' + item.currency + '" data-erpname="' + item.erpName + '" data-enableocr="' + item.enableOCR + '"></option>').val(item.value).html(item.text));
                    }
                });
                var Location_all = $(`${_root} .ddlLocationList option:selected`).map(function () { return $(this).val(); }).get().join(',');
                $(".ddlLocationList option").prop("selected", false);
                if (PChaseModule._PChaseCommon._locationCode !== '' && Location_all.indexOf(PChaseModule._PChaseCommon._locationCode) !== -1) {
                    $.each(PChaseModule._PChaseCommon._locationCode.split(","), function (i, e) {
                        $(`${_root} .ddlLocationList option[value=` + e + `]`).prop("selected", true);
                    });
                    $(`${_root} #ddlLocationList`).trigger('change');
                } else {
                    $.each(Location_all.split(","), function (i, e) {
                        $(`${_root} .ddlLocationList option[value=` + e + `]`).prop("selected", true);
                    });
                    $(`${_root} #ddlLocationList`).trigger('change');
                }
            }, true, false);
        }
    }

    _PChaseCommon.prototype.ClickEventForddlCompanyList = function ($this) {
        $('.otherCompanyList').not($this).prop('checked', false);
    }

    _PChaseCommon.prototype.ChangeEventForddlLocationList = function ($this) {
        PChaseModule._PChaseCommon.updateVariable();
        var CompanyCode = PChaseModule._PChaseCommon._companyCode;
        var LocationCode = PChaseModule._PChaseCommon._locationCode;
        var CompanyRegion = PChaseModule._PChaseCommon._companyRegion;
        let url = module.urls.passUserInfoForOtherProject;
        let data = { 'CompanyCode': CompanyCode, 'CompanyRegion': CompanyRegion, 'LocationCode': LocationCode, 'URL': decodeURIComponent(window.location.pathname + window.location.search) };//is null '{}'
        PChaseModule._PChaseAjaxResponse.GetAjaxJsonResponse(url, data, function (response) {
            if (response.appName === null && response.appCode === null) {
                response.appName = 'Error Page';
                response.appCode = '404Error';
            }
            var _selectedModuleCode = 'Home';
            var _selectedModuleName = response.appName;
            _selectedModuleCode = response.appCode;
            PChaseModule._PChaseCommon._SelectedModuleCode = response.appCode;
            if (response.appCode == "TIMESHEET") {
                $("#layout_company_list_dropdown").hide();
                $("#layout_location_list_drop").hide();
            } else if (response.appCode == "OTF") {
                $("#layout_company_list_dropdown").hide();
                $("#layout_location_list_drop").hide();
            } else if (response.appCode == "Kaizen") {
                $("#layout_company_list_dropdown").hide();
                $("#layout_location_list_drop").hide();
            } else if (response.appCode == "SUPPRECO" && window.location.pathname !== '/SUPPRECO/Dashboard/Index') {
                $("#layout_company_list_dropdown").hide();
                $("#layout_location_list_drop").hide();
            } else if (response.appCode == "Portal" && window.location.pathname !== '/Portal/Home/PchaseWall') {
                $("#layout_company_list_dropdown").hide();
                $("#layout_location_list_drop").hide();
            } else if (response.appCode == "ClientPortal" && window.location.pathname !== '/ClientPortal/Home/PchaseWall') {
                $("#layout_company_list_dropdown").hide();
                $("#layout_location_list_drop").hide();
            } else if (response.appCode == "BACSCONTROL") {
                $("#layout_company_list_dropdown").hide();
                $("#layout_location_list_drop").hide();
            } else {
                $("#layout_company_list_dropdown").show();
                $("#layout_location_list_drop").show();
            }
            $(`${_root} .portal_icon.active`).removeClass('active');
            $(`${_root} .portal_icon.` + _selectedModuleCode).addClass('active');
            if (response.appCode == "TIMESHEET")
                $(`${_root} #Module_Title`).empty().html("Task Manager");
            else
                $(`${_root} #Module_Title`).empty().html(_selectedModuleName);
            if (response.appCode == "PCHASECARDS")
                $(`${_root} #cardsfooter`).removeClass("hidden");
            else
                $(`${_root} #cardsfooter`).addClass("hidden");

            $(`#current-browser-title`).empty().html(_selectedModuleName + ' - Paperchase Accountancy');
            $(`#current-browser-shortcut-icon`).css('href', PChaseModule.configs.imgPath + 'favicon.ico');
            switch (_selectedModuleCode) {
                case "Home":
                    PChaseModule._PChaseCommon.GetPchaseWallPage();
                    $(`${_root} .right_side_bar`).empty();
                    $('body').addClass('fullwidth');
                    break;
                case "SUPPINQ":
                    $(`${_root} #SetDynemicModule`).empty();
                    $(`${_root} #SetDynemicModule`).html('');
                    $(`${_root} #SetDynemicModule`).html("<div class='global_wrapper' id='partial-wrapper-suppinq'><div class='module_wrapper'><div class='iframe-loading'><iframe id='iframeSUPPINQViewer' class='pdf-img' style='max-width: 100%;height:calc(100vh - 110px);width: 100%;' src='' onload='$('.iframe-loading').css('background-image', 'none');'></iframe></div></div></div>");
                    var params = { 'CtxUser': _configs.loginUserName, 'UserType': _configs.loginUserType, 'Rights': _configs.loginUserRights, 'CompanyCode': CompanyCode, 'SiteCode': LocationCode, 'AppFilePath': PChaseModule._PChaseCommon._companyAppFilePath };
                    var _url = response.url + '?' + jQuery.param(params);
                    $('#iframeSUPPINQViewer').empty();
                    $('#iframeSUPPINQViewer').attr('src', _url);
                    $('#iframeSUPPINQViewer').on('load', function () {
                        PChaseModule._PChaseLoader.LoaderEnd();
                    });
                    break;
                case "DEBTORINQ":
                    $(`${_root} #SetDynemicModule`).empty();
                    $(`${_root} #SetDynemicModule`).html('');
                    $(`${_root} #SetDynemicModule`).html("<div class='global_wrapper' id='partial-wrapper-debtorinq'><div class='module_wrapper'><div class='iframe-loading'><iframe id='iframeDEBTORINQViewer' class='pdf-img' style='max-width: 100%;height:calc(100vh - 110px);width: 100%;' src='' onload='$('.iframe-loading').css('background-image', 'none');'></iframe></div></div></div>");
                    var params = { 'CtxUser': _configs.loginUserName, 'UserType': _configs.loginUserType, 'Rights': _configs.loginUserRights, 'CompanyCode': CompanyCode, 'SiteCode': LocationCode, 'AppFilePath': PChaseModule._PChaseCommon._companyAppFilePath };
                    var _url = response.url + '?' + jQuery.param(params);
                    $('#iframeDEBTORINQViewer').empty();
                    $('#iframeDEBTORINQViewer').attr('src', _url);
                    $('#iframeDEBTORINQViewer').on('load', function () {
                        PChaseModule._PChaseLoader.LoaderEnd();
                    });
                    break;
                case "WEBNOMINAL":
                    $(`${_root} #SetDynemicModule`).empty();
                    $(`${_root} #SetDynemicModule`).html('');
                    $(`${_root} #SetDynemicModule`).html("<div class='global_wrapper' id='partial-wrapper-webnominal'><div class='module_wrapper'><div class='iframe-loading'><iframe id='iframeWEBNOMINALViewer' class='pdf-img' style='max-width: 100%;height:calc(100vh - 110px);width: 100%;' src='' onload='$('.iframe-loading').css('background-image', 'none');'></iframe></div></div></div>");
                    var params = { 'CtxUser': _configs.loginUserName, 'UserType': _configs.loginUserType, 'Rights': _configs.loginUserRights, 'CompanyCode': CompanyCode, 'SiteCode': LocationCode, 'AppFilePath': PChaseModule._PChaseCommon._companyAppFilePath };
                    var _url = response.url + '?' + jQuery.param(params);
                    $('#iframeWEBNOMINALViewer').empty();
                    $('#iframeWEBNOMINALViewer').attr('src', _url);
                    $('#iframeWEBNOMINALViewer').on('load', function () {
                        PChaseModule._PChaseLoader.LoaderEnd();
                    });
                    break;
                case "INVMANAGER":
                    $(`${_root} #SetDynemicModule`).empty();
                    $(`${_root} #SetDynemicModule`).html('');
                    $(`${_root} #SetDynemicModule`).html("<div class='global_wrapper' id='partial-wrapper-invmanager'><div class='module_wrapper'><div class='iframe-loading'><iframe id='iframeINVMANAGERViewer' class='pdf-img' style='max-width: 100%;height:calc(100vh - 110px);width: 100%;' src='' onload='$('.iframe-loading').css('background-image', 'none');'></iframe></div></div></div>");
                    // var params = { 'CtxUser': _configs.loginUserName, 'UserType': _configs.loginUserType, 'Rights': _configs.loginUserRights, 'CompanyCode': CompanyCode, 'SiteCode': LocationCode, 'AppFilePath': PChaseModule._PChaseCommon._companyAppFilePath };
                    var params = { 'CtxUser': _configs.loginUserName, 'UserType': _configs.loginUserType, 'Rights': _configs.loginUserRights }; /*, 'CompanyCode': CompanyCode, 'SiteCode': LocationCode, 'AppFilePath': PChaseModule._PChaseCommon._companyAppFilePath*/
                    var _url = response.url + '?' + jQuery.param(params);
                    $('#iframeINVMANAGERViewer').empty();
                    $('#iframeINVMANAGERViewer').attr('src', _url);
                    $('#iframeINVMANAGERViewer').on('load', function () {
                        PChaseModule._PChaseLoader.LoaderEnd();
                    });
                    break;
                case "GLMAP":
                    $(`${_root} #SetDynemicModule`).empty();
                    $(`${_root} #SetDynemicModule`).html('');
                    $(`${_root} #SetDynemicModule`).html("<div class='global_wrapper' id='partial-wrapper-glmap'><div class='module_wrapper'><div class='iframe-loading'><iframe id='iframeGLMAPViewer' class='pdf-img' style='max-width: 100%;height:calc(100vh - 110px);width: 100%;' src='' onload='$('.iframe-loading').css('background-image', 'none');'></iframe></div></div></div>");
                    var params = { 'Username': _configs.loginUserName, 'UserType': _configs.loginUserType, 'companyCode': CompanyCode };
                    var _url = response.url.replace('default.aspx','') + '?' + jQuery.param(params);
                    $('#iframeGLMAPViewer').empty();
                    $('#iframeGLMAPViewer').attr('src', _url);
                    $('#iframeGLMAPViewer').on('load', function () {
                        PChaseModule._PChaseLoader.LoaderEnd();
                    });
                    break;
                case "AICHATBOT":
                    $(`${_root} #SetDynemicModule`).empty();
                    $(`${_root} #SetDynemicModule`).html('');
                    $(`${_root} #SetDynemicModule`).html("<div class='global_wrapper' id='partial-wrapper-suppinq'><div class='module_wrapper'><div class='iframe-loading'><iframe id='iframeAICHATBOTViewer' class='pdf-img' style='max-width: 100%;height:calc(100vh - 110px);width: 100%;' src='' onload='$('.iframe-loading').css('background-image', 'none');'></iframe></div></div></div>");
                    var params = { 'UserType': _configs.loginUserType, 'companycode': CompanyCode, 'sitecode': LocationCode };
                    var _url = response.url + '?' + jQuery.param(params);
                    $('#iframeAICHATBOTViewer').empty();
                    $('#iframeAICHATBOTViewer').attr('src', _url);
                    $('#iframeAICHATBOTViewer').on('load', function () {
                        PChaseModule._PChaseLoader.LoaderEnd();
                    });
                    break;
                case "404Error":
                    $(`${_root} #SetDynemicModule`).empty();
                    $(`${_root} #SetDynemicModule`).html('');
                    $(`${_root} #SetDynemicModule`).html("<div class='global_wrapper' id='partial-wrapper-suppinq'><div class='module_wrapper'><div class='iframe-loading'><iframe id='iframe404ErrorViewer' class='pdf-img' style='max-width: 100%;height:calc(100vh - 110px);width: 100%;' src='' onload='$('.iframe-loading').css('background-image', 'none');'></iframe></div></div></div>");
                    $('#iframe404ErrorViewer').empty();
                    $('#iframe404ErrorViewer').attr('src', 'https://web.paperchase.ac/404.html');
                    $('#iframe404ErrorViewer').on('load', function () {
                        PChaseModule._PChaseLoader.LoaderEnd();
                    });
                    break;
                default:
                    var currentBrowserUrl = response.url;//PChaseModule._PChaseCommon._SetCurrentBrowserUrl;
                    if (currentBrowserUrl !== "") {
                        PChaseModule._PChaseCommon._GetCurrentProjectBaseUrl = response.webURL;
                        PChaseModule._PChaseCommon.Common_DocumentReady(currentBrowserUrl);
                    }
                    $(`${_root} .right_side_bar`).empty();
                    $('body').addClass('fullwidth');
                    break;
            }
            $(`${_root} .side_menu_tab_content`).removeClass('show');
        }, true, false);
    }

    _PChaseCommon.prototype.GetPchaseWallPage = function () {
        var CompanyCode = PChaseModule._PChaseCommon._companyCode;
        var LocationCode = PChaseModule._PChaseCommon._locationCode;
        let url = module.urls.pchaseWallPage;
        let data = { 'CompanyCode': CompanyCode, 'LocationCode': LocationCode };
        PChaseModule._PChaseAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
            $(`${_root} #SetDynemicModule`).empty();
            $(`${_root} #SetDynemicModule`).empty().html(response);
        });
    }

    _PChaseCommon.prototype.BindDocmanagerMenuList = function () {
        let url = module.urls.bindDocmanagerMenuList;
        let data = '{}';
        PChaseModule._PChaseAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
            $(`${_root} #CPortal_DocManagerMenu`).empty();
            $(`${_root} #CPortal_DocManagerMenu`).empty().html(response);
            if (localStorage.getItem("hdnSetCurrentTabName") !== null) {
                $(`${_root} .side_menu_panel_scroll a.timelog.active`).removeClass('active');
                $(`${_root} .side_menu_panel_scroll a.timelog.` + localStorage.getItem("hdnSetCurrentTabName")).addClass('active');
            } else {
                $(`${_root} .side_menu_panel_scroll a.timelog.active`).removeClass('active');
                $(`${_root} .side_menu_panel_scroll a.timelog.` + sessionStorage.getItem("CPortalSetCurrentTabName")).addClass('active');
            }
        });
    }

    _PChaseCommon.prototype.BindTaskmanagerMenuList = function () {
        let url = module.urls.bindTaskmanagerMenuList;
        let data = '{}';
        PChaseModule._PChaseAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
            $(`${_root} #CPortal_TaskManagerMenu`).empty();
            $(`${_root} #CPortal_TaskManagerMenu`).empty().html(response);
            if (localStorage.getItem("hdnSetCurrentTabName") !== null) {
                $(`${_root} .side_menu_panel_scroll a.timelog.active`).removeClass('active');
                $(`${_root} .side_menu_panel_scroll a.timelog.` + localStorage.getItem("hdnSetCurrentTabName")).addClass('active');
            } else {
                $(`${_root} .side_menu_panel_scroll a.timelog.active`).removeClass('active');
                $(`${_root} .side_menu_panel_scroll a.timelog.` + sessionStorage.getItem("CPortalSetCurrentTabName")).addClass('active');
            }
        });
    }

    _PChaseCommon.prototype.BindManagementAccountMenuList = function () {
        let url = module.urls.bindManagementAccountMenuList;
        let data = '{}';
        PChaseModule._PChaseAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
            $(`${_root} #CPortal_ManagementAccounMenu`).empty();
            $(`${_root} #CPortal_ManagementAccounMenu`).empty().html(response);
            if (localStorage.getItem("hdnSetCurrentTabName") !== null) {
                $(`${_root} .side_menu_panel_scroll a.timelog.active`).removeClass('active');
                $(`${_root} .side_menu_panel_scroll a.timelog.` + localStorage.getItem("hdnSetCurrentTabName")).addClass('active');
            } else {
                $(`${_root} .side_menu_panel_scroll a.timelog.active`).removeClass('active');
                $(`${_root} .side_menu_panel_scroll a.timelog.` + sessionStorage.getItem("CPortalSetCurrentTabName")).addClass('active');
            }
        });
    }

    _PChaseCommon.prototype.BindSupplierRecoMenuList = function () {
        let url = module.urls.bindSupplierRecoMenuList;
        let data = '{}';
        var response = PChaseModule._PChaseAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
            $(`${_root} #CPortal_SupplierRecoMenu`).empty();
            $(`${_root} #CPortal_SupplierRecoMenu`).empty().html(response);
            if (localStorage.getItem("hdnSetCurrentTabName") !== null) {
                $(`${_root} .side_menu_panel_scroll a.timelog.active`).removeClass('active');
                $(`${_root} .side_menu_panel_scroll a.timelog.` + localStorage.getItem("hdnSetCurrentTabName")).addClass('active');
            } else {
                $(`${_root} .side_menu_panel_scroll a.timelog.active`).removeClass('active');
                $(`${_root} .side_menu_panel_scroll a.timelog.` + sessionStorage.getItem("CPortalSetCurrentTabName")).addClass('active');
            }
        });
    }

    _PChaseCommon.prototype.SetSubMenuData = function (tab) {
        if ($(`${_root} .side_menu_tab_content`).hasClass('show')) {
            $(`${_root} .side_menu_tab_content`).removeClass('show');
            $(`${_root} #` + tab).attr("style", "display:none;");
            $(`${_root} #` + tab).empty();
        } else {
            $(`${_root} #` + tab).attr("style", "display:block;");
            switch (tab) {
                case "CPortal_DocManagerMenu":
                    $(`${_root} #CPortal_TaskManagerMenu`).attr("style", "display:none;");
                    $(`${_root} #CPortal_SupplierRecoMenu`).attr("style", "display:none;");
                    $(`${_root} #CPortal_ManagementAccounMenu`).attr("style", "display:none;");
                    PChaseModule._PChaseCommon.BindDocmanagerMenuList();
                    break;
                case "CPortal_TaskManagerMenu":
                    $(`${_root} #CPortal_DocManagerMenu`).attr("style", "display:none;");
                    $(`${_root} #CPortal_SupplierRecoMenu`).attr("style", "display:none;");
                    $(`${_root} #CPortal_ManagementAccounMenu`).attr("style", "display:none;");
                    PChaseModule._PChaseCommon.BindTaskmanagerMenuList();
                    break;
                case "CPortal_SupplierRecoMenu":
                    $(`${_root} #CPortal_DocManagerMenu`).attr("style", "display:none;");
                    $(`${_root} #CPortal_TaskManagerMenu`).attr("style", "display:none;");
                    $(`${_root} #CPortal_ManagementAccounMenu`).attr("style", "display:none;");
                    PChaseModule._PChaseCommon.BindSupplierRecoMenuList();
                    break;
                case "CPortal_ManagementAccounMenu":
                    $(`${_root} #CPortal_DocManagerMenu`).attr("style", "display:none;");
                    $(`${_root} #CPortal_TaskManagerMenu`).attr("style", "display:none;");
                    $(`${_root} #CPortal_SupplierRecoMenu`).attr("style", "display:none;");
                    PChaseModule._PChaseCommon.BindManagementAccountMenuList();
                    break;
            }
            $(`${_root} .side_menu_tab_content`).addClass('show');
        }
    }

    _PChaseCommon.prototype.SetCurrentTabName = function (id) {
        localStorage.setItem("hdnSetCurrentTabName", id);
        sessionStorage.setItem("CPortalSetCurrentTabName", id);
    }

    _PChaseCommon.prototype.Common_DocumentReady = function (url) {
        PChaseModule._PChaseLoader.LoaderEnd();
        $.ajax({
            type: "POST",
            url: url,
            data: '{}',
            headers: {
                "Accept-Language": PChaseModule.configs.loginUserCultureCode,
                "App-CompanyCode": PChaseModule._PChaseCommon._companyCode,
                "App-SiteCode": PChaseModule._PChaseCommon._locationCode,
                "App-CompanyRegion": PChaseModule._PChaseCommon._companyRegion,
                "App-ERPName": PChaseModule._PChaseCommon._erpName,
                "App-EnableOCR": PChaseModule._PChaseCommon._enableOCR,
                "App-NumberOfDecimal": PChaseModule._PChaseCommon._companyNumberOfDecimal,
                "Access-Control-Allow-Origin": PChaseModule.configs.basePath,
                "Access-Control-Allow-Credentials": true,
                "Access-Control-Allow-Methods": 'GET,PUT,POST,DELETE',
                "Access-Control-Allow-Headers": "Origin, X-Requested-With, Content-Type, Accept"
            },
            dataType: 'html',
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            success: function (data) {
                $(`${_root} #SetDynemicModule`).empty();
                $(`${_root} #SetDynemicModule`).html('');
                $(`${_root} #SetDynemicModule`).html(data);
                PChaseModule._PChaseLoader.LoaderEnd();
            },
            error: function ajaxError(response) {
                PChaseModule._PChanseToaster._error(response.responseText);
            },
            failure: function (response) {
                PChaseModule._PChanseToaster._failure(response.responseText);
            }
        });
    }

    //Check for value ex. return ''
    _PChaseCommon.prototype.isNullOrEmptyV = function (value) {
        let str = '';
        if (!value
            || value == null
            || value === 'null'
            || value === ''
            || value === ""
            || value === '{}'
            || value === 'undefined'
            || value === undefined
            || value.length === 0) return str;
    }

    //Check for condition ex. if
    _PChaseCommon.prototype.isNullOrEmpty = function (value) {
        return !value
            || value == null
            || value === 'null'
            || value === ''
            || value === ""
            || value == "0"
            || value === '{}'
            || value === 'undefined'
            || value === undefined
            || value.length === 0;
    }

    _PChaseCommon.prototype.isNullOrEmptyOrZero = function (value) {
        return (value == null || value == undefined || value == "" || value == "0");
    }

    _PChaseCommon.prototype.RptCollopseShowHide = function ($this) {
        var _tabCode = $($this).attr('data-tabCode');
        $('.Child_Row_' + _tabCode).toggleClass('hidden');
        if ($('.Child_Row_' + _tabCode).hasClass('hidden')) {
            $('#img-show-rpt', $this).removeClass('hidden');
            $('#img-hide-rpt', $this).addClass('hidden');
        } else {
            $('#img-show-rpt', $this).addClass('hidden');
            $('#img-hide-rpt', $this).removeClass('hidden');
        }
        /* $('.scroll-body').scrollTop(0);*/
        $('.scroll-body').perfectScrollbar('update');
    }

    _PChaseCommon.prototype.CatCollopseShowHide = function ($this) {
        var _tabCode = $($this).attr('data-catcode');
        $('.cat_' + _tabCode).toggleClass('hidden');
        if ($('.category_row').hasClass('hidden')) {
            $('#img-show-cat', $this).removeClass('hidden');
            $('#img-hide-cat', $this).addClass('hidden');
        } else {
            $('#img-show-cat', $this).addClass('hidden');
            $('#img-hide-cat', $this).removeClass('hidden');
        }
        /*     $('.scroll-body').scrollTop(0);*/
        $('.scroll-body').perfectScrollbar('update');
    }

    _PChaseCommon.prototype.InvoiceViewer = function ($this) {
        $('.table > tbody > tr').removeClass('active');
        $($this).closest('tr').addClass('active');
        let url = _configs.basePath + '/InvoiceViewer/Index';
        let data = {
            CompanyCode: PChaseModule._PChaseCommon.isNullOrEmpty($($this).attr('data-ccode')) ? module._PChaseCommon._companyCode : $($this).attr('data-ccode'),
            SiteCode: $($this).attr('data-scode'),
            InvoiceNo: $($this).attr('data-invno'),
            FileName: $($this).attr('data-fname'),
            PageType: PChaseModule._PChaseCommon.isNullOrEmpty($($this).attr('data-ptype')) ? '' : $($this).attr('data-ptype')
        };
        PChaseModule._PChaseAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
            $(`${_root} #common-invoice-viewer`).empty();
            $(`${_root} #common-invoice-viewer`).html(response);
            $(`${_root} #common-invoice-viewer`).modal('show');
            $(`${_root} #ScrollMoreInvoiceViewer`).scrollTop(0);
            $(`${_root} #ScrollMoreInvoiceViewer`).perfectScrollbar("destroy");
            $(`${_root} #ScrollMoreInvoiceViewer`).perfectScrollbar();
            $(`${_root} #ScrollMoreInvoiceView_Page`).scrollTop(0);
            $(`${_root} #ScrollMoreInvoiceView_Page`).perfectScrollbar("destroy");
            $(`${_root} #ScrollMoreInvoiceView_Page`).perfectScrollbar();
            $(`${_root} #ScrollMoreInvoiceGL_Page`).scrollTop(0);
            $(`${_root} #ScrollMoreInvoiceGL_Page`).perfectScrollbar("destroy");
            $(`${_root} #ScrollMoreInvoiceGL_Page`).perfectScrollbar();
            if (PChaseModule._PChaseCommon._SelectedModuleCode === 'INVPAYNEW') {
                if (parseInt(window.parent.$('.modalTblWrapper .table > tbody > tr').length) > 0) {
                    var row_index = parseInt(window.parent.$('.modalTblWrapper .table > tbody > tr.active').index());
                    if (window.parent.$('.modalTblWrapper .table > tbody > tr:eq(' + row_index + ') td input[type="checkbox"]').is(":checked") === true) {
                        $(".IAS_chk_ref_no").prop("checked", true);
                    } else {
                        $(".IAS_chk_ref_no").prop("checked", false);
                    }
                } else {
                    if (parseInt(window.parent.$('.tableWrapper .table > tbody > tr').length) > 0) {
                        var row_index = parseInt(window.parent.$('.tableWrapper .table > tbody > tr.active').index());
                        if (window.parent.$('.tableWrapper .table > tbody > tr:eq(' + row_index + ') td input[type="checkbox"]').is(":checked") === true) {
                            $(".IAS_chk_ref_no").prop("checked", true);
                        } else {
                            $(".IAS_chk_ref_no").prop("checked", false);
                        }
                    }
                }
                // Prev button hide in invoice viewer
                if (parseInt(window.parent.$('.modalTblWrapper .table > tbody > tr').length) > 0) {
                    if (parseInt(window.parent.$('.modalTblWrapper .table > tbody > tr.active').index()) === 0) {
                        window.parent.$('#btn_IASInvoiceViewerPrev').attr('disabled', true);
                    } else {
                        window.parent.$('#btn_IASInvoiceViewerPrev').removeAttr('disabled');
                    }
                } else {
                    if (parseInt(window.parent.$('.tableWrapper .table > tbody > tr').length) > 0) {
                        if (parseInt(window.parent.$('.tableWrapper .table > tbody > tr.active').index()) === 0) {
                            window.parent.$('#btn_IASInvoiceViewerPrev').attr('disabled', true);
                        } else {
                            window.parent.$('#btn_IASInvoiceViewerPrev').removeAttr('disabled');
                        }
                    } else {
                        if (parseInt(window.parent.$('.tblWrapper .table > tbody > tr').length) > 0) {
                            if (parseInt(window.parent.$('.tblWrapper .table > tbody > tr.active').index()) === 0) {
                                window.parent.$('#btn_IASInvoiceViewerPrev').attr('disabled', true);
                            } else {
                                window.parent.$('#btn_IASInvoiceViewerPrev').removeAttr('disabled');
                            }
                        }
                    }
                }
                // Next button hide in invoice viewer
                if (parseInt(window.parent.$('.modalTblWrapper .table > tbody > tr').length) > 0) {
                    if ((parseInt(window.parent.$('.modalTblWrapper .table > tbody > tr.active').index()) + 1) === parseInt(window.parent.$('.modalTblWrapper .table > tbody > tr').length)) {
                        window.parent.$('#btn_IASInvoiceviewerNext').attr('disabled', true);
                    } else {
                        window.parent.$('#btn_IASInvoiceviewerNext').removeAttr('disabled');
                    }
                } else {
                    if (parseInt(window.parent.$('.tableWrapper .table > tbody > tr').length) > 0) {
                        if ((parseInt(window.parent.$('.tableWrapper .table > tbody > tr.active').index()) + 1) === parseInt(window.parent.$('.tableWrapper .table > tbody > tr').length)) {
                            window.parent.$('#btn_IASInvoiceviewerNext').attr('disabled', true);
                        } else {
                            window.parent.$('#btn_IASInvoiceviewerNext').removeAttr('disabled');
                        }
                    } else {
                        if (parseInt(window.parent.$('.tblWrapper .table > tbody > tr').length) > 0) {
                            if ((parseInt(window.parent.$('.tblWrapper .table > tbody > tr.active').index()) + 1) === parseInt(window.parent.$('.tblWrapper .table > tbody > tr').length)) {
                                window.parent.$('#btn_IASInvoiceviewerNext').attr('disabled', true);
                            } else {
                                window.parent.$('#btn_IASInvoiceviewerNext').removeAttr('disabled');
                            }
                        }
                    }
                }
                $("#Div_ref_no").removeClass('hidden');
            } else {
                $("#Div_ref_no").addClass('hidden');
                window.parent.$('#btn_IASInvoiceviewerNext').attr('disabled', true);
                window.parent.$('#btn_IASInvoiceViewerPrev').attr('disabled', true);
            }
        });
    }

    _PChaseCommon.prototype.CloseInvoiceViewer = function ($this) {
        $(`${_root} #common-invoice-viewer`).empty();
        $(`${_root} #common-invoice-viewer`).modal('hide');
    }
    // Prev button to move invoice viewer
    _PChaseCommon.prototype.ClickLeftBtnInvViewer = function () {
        if (parseInt(window.parent.$('.modalTblWrapper .table > tbody > tr').length) > 0) {
            if (parseInt(window.parent.$('.modalTblWrapper .table > tbody > tr.active').index()) > 0) {
                var row_index = parseInt(window.parent.$('.modalTblWrapper .table > tbody > tr.active').index()) - 1;
                window.parent.$('.modalTblWrapper .table > tbody > tr:eq(' + row_index + ') td a[onclick="PChaseModule._PChaseCommon.InvoiceViewer(this);"]').click();
                window.parent.$('.modalTblWrapper .table > tbody > tr').removeClass('active');
                window.parent.$('.modalTblWrapper .table > tbody > tr:eq(' + row_index + ')').addClass('active');
            }
        } else {
            if (parseInt(window.parent.$('.tableWrapper .table > tbody > tr').length) > 0) {
                if (parseInt(window.parent.$('.tableWrapper .table > tbody > tr.active').index()) > 0) {
                    var row_index = parseInt(window.parent.$('.tableWrapper .table > tbody > tr.active').index()) - 1;
                    window.parent.$('.tableWrapper .table > tbody > tr:eq(' + row_index + ') td a[onclick="PChaseModule._PChaseCommon.InvoiceViewer(this);"]').click();
                    window.parent.$('.tableWrapper .table > tbody > tr').removeClass('active');
                    window.parent.$('.tableWrapper .table > tbody > tr:eq(' + row_index + ')').addClass('active');
                }
            } else {
                if (parseInt(window.parent.$('.tblWrapper .table > tbody > tr').length) > 0) {
                    if (parseInt(window.parent.$('.tblWrapper .table > tbody > tr.active').index()) > 0) {
                        var row_index = parseInt(window.parent.$('.tblWrapper .table > tbody > tr.active').index()) - 1;
                        window.parent.$('.tblWrapper .table > tbody > tr:eq(' + row_index + ') td a[onclick="PChaseModule._PChaseCommon.InvoiceViewer(this);"]').click();
                        window.parent.$('.tblWrapper .table > tbody > tr').removeClass('active');
                        window.parent.$('.tblWrapper .table > tbody > tr:eq(' + row_index + ')').addClass('active');
                    }
                }
            }
        }
    }
    //Next button to move invoice viewer
    _PChaseCommon.prototype.ClickRigthBtnInvViewer = function () {
        if (parseInt(window.parent.$('.modalTblWrapper .table > tbody > tr').length) > 0) {
            if ((parseInt(window.parent.$('.modalTblWrapper .table > tbody > tr.active').index()) + 1) !== parseInt(window.parent.$('.modalTblWrapper .table > tbody > tr').length)) {
                var row_index = parseInt(window.parent.$('.modalTblWrapper .table > tbody > tr.active').index()) + 1;
                window.parent.$('.modalTblWrapper .table > tbody > tr:eq(' + row_index + ') td a[onclick="PChaseModule._PChaseCommon.InvoiceViewer(this);"]').click();
                window.parent.$('.modalTblWrapper .table > tbody > tr').removeClass('active');
                window.parent.$('.modalTblWrapper .table > tbody > tr:eq(' + row_index + ')').addClass('active');
            }
        } else {
            if (parseInt(window.parent.$('.tableWrapper .table > tbody > tr').length) > 0) {
                if ((parseInt(window.parent.$('.tableWrapper .table > tbody > tr.active').index()) + 1) !== parseInt(window.parent.$('.tableWrapper .table > tbody > tr').length)) {
                    var row_index = parseInt(window.parent.$('.tableWrapper .table > tbody > tr.active').index()) + 1;
                    window.parent.$('.tableWrapper .table > tbody > tr:eq(' + row_index + ') td a[onclick="PChaseModule._PChaseCommon.InvoiceViewer(this);"]').click();
                    window.parent.$('.tableWrapper .table > tbody > tr').removeClass('active');
                    window.parent.$('.tableWrapper .table > tbody > tr:eq(' + row_index + ')').addClass('active');
                }
            } else {
                if (parseInt(window.parent.$('.tblWrapper .table > tbody > tr').length) > 0) {
                    if ((parseInt(window.parent.$('.tblWrapper .table > tbody > tr.active').index()) + 1) !== parseInt(window.parent.$('.tblWrapper .table > tbody > tr').length)) {
                        var row_index = parseInt(window.parent.$('.tblWrapper .table > tbody > tr.active').index()) + 1;
                        window.parent.$('.tblWrapper .table > tbody > tr:eq(' + row_index + ') td a[onclick="PChaseModule._PChaseCommon.InvoiceViewer(this);"]').click();
                        window.parent.$('.tblWrapper .table > tbody > tr').removeClass('active');
                        window.parent.$('.tblWrapper .table > tbody > tr:eq(' + row_index + ')').addClass('active');
                    }
                }
            }
        }
    }
    //Next button to move invoice viewer
    _PChaseCommon.prototype.IAS_Chk_InvoiceViewer = function ($this) {
        if ($($this).is(":checked")) {
            if (parseInt(window.parent.$('.modalTblWrapper .table > tbody > tr').length) > 0) {
                var row_index = parseInt(window.parent.$('.modalTblWrapper .table > tbody > tr.active').index());
                window.parent.$('.modalTblWrapper .table > tbody > tr:eq(' + row_index + ') td input[type="checkbox"]').prop("checked", true).change();
            } else {
                if (parseInt(window.parent.$('.tableWrapper .table > tbody > tr').length) > 0) {
                    var row_index1 = parseInt(window.parent.$('.tableWrapper .table > tbody > tr.active').index());
                    window.parent.$('.tableWrapper .table > tbody > tr:eq(' + row_index1 + ') td input[type="checkbox"]').prop("checked", true).change();
                }
            }
        } else {
            if (parseInt(window.parent.$('.modalTblWrapper .table > tbody > tr').length) > 0) {
                var row_index2 = parseInt(window.parent.$('.modalTblWrapper .table > tbody > tr.active').index());
                window.parent.$('.modalTblWrapper .table > tbody > tr:eq(' + row_index2 + ') td input[type="checkbox"]').prop("checked", false).change();
            } else {
                if (parseInt(window.parent.$('.tableWrapper .table > tbody > tr').length) > 0) {
                    var row_index3 = parseInt(window.parent.$('.tableWrapper .table > tbody > tr.active').index());
                    window.parent.$('.tableWrapper .table > tbody > tr:eq(' + row_index3 + ') td input[type="checkbox"]').prop("checked", false).change();
                }
            }
        }
    }

    _PChaseCommon.prototype.GlobalSearchClick = function () {
        $(".globalSearchWrapper").slideToggle("slow");
        $('#txtGlobalSearch').focus();
    }

    _PChaseCommon.prototype.TextGlobalSearch = function ($this, event) {
        if (event.keyCode === 13) {
            if ($($this).val().length > 2) {
                event.preventDefault();
                var searchText = $($this).val();
                var pageInd = $("#hdnGSPageIndex").val();
                var dflpagesize = $("#hdnGSDflPageSize").val();
                var orderby = $("#hdnGSOrderBy").val();
                var ordertype = $("#hdnGSOrderType").val();
                PChaseModule._PChaseCommon.ListInvoices('', '', '', '', searchText, orderby, ordertype, pageInd, dflpagesize);
            }
            else {
                PChaseModule._PChanseToaster._toastr(2, 'Please enter atlest 3 character.');
            }
        }

    }

    _PChaseCommon.prototype.ListInvoices = function (userName, userType, siteCode, companyCode, searchTerm, orderBy, orderType, pageIndex, pageSize) {
        let url = _configs.basePath + '/InvoiceViewer/GlobalInvoiceSearchList/';
        let data = {
            siteCode: PChaseModule._PChaseCommon._locationCode,
            companyCode: PChaseModule._PChaseCommon._companyCode,
            userName: userName,
            userType: userType,
            SearchTerm: searchTerm,
            PageIndex: pageIndex,
            PageSize: pageSize,
            OrderBy: orderBy,
            OrderType: orderType
        };
        PChaseModule._PChaseAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
            $(".global-search-res").html('');
            $(".global-search-res").html(response);
            if (pageSize == 100) {
                $('#globalsearchinvoicelist').scrollTop(0);
            }
            if ($('#hdnGSTotalResultInv').val() == undefined) {
                $('#gsspnSearchResult').html($('#hdnSearchResultTitle').val() + ' 0');
            }
            else {
                $('#gsspnSearchResult').html($('#hdnSearchResultTitle').val() + ' ' + $('#hdnGSTotalResultInv').val());
            }
        });
    }

    _PChaseCommon.prototype.closeGlobalSearch = function () {
        $('#hdnSearchResultTitle').val('');
        $('#hdnGSTotalResultInv').val('');
        $('#gsspnSearchResult').html('');
        $('#txtGlobalSearch').val('');
        $('.globalSearchList').empty();
        $(".globalSearchWrapper").slideToggle("slow");
    }

    _PChaseCommon.prototype.ScrollForGlobalInvoiceList = function (event, $this) {
        if ($($this).scrollTop() + $($this).innerHeight() + 5 >= $($this)[0].scrollHeight) {
            var tbl = document.getElementById("globalInvList");
            var trCnt = tbl.getElementsByTagName("tr").length;
            var totalResultInv = $("#hdnGSTotalResultInv").val();
            if (trCnt > 0) {
                var dataCnt = (parseFloat(trCnt)); //- parseFloat(1));
                if (parseFloat(dataCnt) < parseFloat(totalResultInv)) {
                    var dflpagesize = $("#hdnGSDflPageSize").val();
                    var invCnt = (parseFloat(dataCnt) + parseFloat(dflpagesize)).toFixed(0);
                    $("#hdnGSPageSize").val(invCnt);
                    var pageInd = $("#hdnGSPageIndex").val();
                    var pageSize = $("#hdnGSPageSize").val();

                    var searchText = $('#txtGlobalSearch').val();
                    var pageInd = $("#hdnGSPageIndex").val();
                    var dflpagesize = $("#hdnGSDflPageSize").val();
                    var orderby = $("#hdnGSOrderBy").val();
                    var ordertype = $("#hdnGSOrderType").val();
                    PChaseModule._PChaseCommon.ListInvoices('', '', '', '', searchText, orderby, ordertype, pageInd, pageSize);
                }
                else {
                }
            }
        }
    }

    _PChaseCommon.prototype.GetReviewCountDashboard = function () {
        let url = module.urls.bindReviewNofiticationCount;
        let data = '{}';
        PChaseModule._PChaseAjaxResponse.GetAjaxJsonResponse(url, data, function (response) {
            if (response === null) { PChaseModule._PChanseToaster._toastr(0, 'Your response is not found!'); }
            if (response !== null) {
                $('.notify_count_reviewCount').text(response.reviewCount);
                $('.bell_noti_FirstSchRCount').text(response.firstReviewScheduleCount);
                $('.bell_noti_FirstAhocRCount').text(response.adhocReviewCount);
                $('.bell_noti_SecondSchRCount').text(response.secondReviewScheduleCount);
                $('.bell_noti_ReviewDate').text(response.lastReviewDate);
                $('.bell_noti_PenReviewDate').text(response.pendingReviewDate);
                $(`${_root} #taskManagerReviewCount`).attr('data-rcount', response.reviewCount);
                $(`${_root} #taskManagerReviewCount`).attr('data-frscount', response.firstReviewScheduleCount);
                $(`${_root} #taskManagerReviewCount`).attr('data-fracount', response.adhocReviewCount);
                $(`${_root} #taskManagerReviewCount`).attr('data-srscount', response.secondReviewScheduleCount);
                PChaseModule._PChaseCommon.HideShowReviewNotification();
            }
        });

    }

    _PChaseCommon.prototype.Clicktlogo_icon = function () {
        $("#common-cportallogin-wrapper").toggleClass('fullScreenView');
    }


    _PChaseCommon.prototype.InvoicePOViewer = function ($this) {
        $('.table > tbody > tr').removeClass('active');
        $($this).closest('tr').addClass('active');
        let url = _configs.basePath + '/InvoiceViewer/POInvoice';
        let data = {
            CompanyCode: PChaseModule._PChaseCommon.isNullOrEmpty($($this).attr('data-ccode')) ? module._PChaseCommon._companyCode : $($this).attr('data-ccode'),
            TxnId: $($this).attr('data-txnid'),
            FileName: $($this).attr('data-fname')
        };
        PChaseModule._PChaseAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
            $(`${_root} #common-invoice-viewer`).empty();
            $(`${_root} #common-invoice-viewer`).html(response);
            $(`${_root} #common-invoice-viewer`).modal('show');
            $(`${_root} #ScrollMoreInvoicePOViewer`).scrollTop(0);
            $(`${_root} #ScrollMoreInvoicePOViewer`).perfectScrollbar("destroy");
            $(`${_root} #ScrollMoreInvoicePOViewer`).perfectScrollbar();
            $(`${_root} #ScrollMoreInvoicePOView_Page`).scrollTop(0);
            $(`${_root} #ScrollMoreInvoicePOView_Page`).perfectScrollbar("destroy");
            $(`${_root} #ScrollMoreInvoicePOView_Page`).perfectScrollbar();
            $(`${_root} #ScrollMoreInvoicePOGL_Page`).scrollTop(0);
            $(`${_root} #ScrollMoreInvoicePOGL_Page`).perfectScrollbar("destroy");
            $(`${_root} #ScrollMoreInvoicePOGL_Page`).perfectScrollbar();
        });
    }

    _PChaseCommon.prototype.CloseCompanyDropDownList = function () {
        $('#company-dropdown-menu-render').hide();
    }

    _PChaseCommon.prototype.ChangeCompanyDropDownList = function () {
        $('.group-checkbox').prop('checked', false);
        $(".childGroupCompany .company-checkbox:checked").each(function () {
            $(this).prop('checked', false);
        });
        var $group = $('.group:eq(0)');
        $group.find('.group-checkbox').prop('checked', false).first().change();
    }

    _PChaseCommon.prototype.CriteriaIADDL = function ($this) {
        var val = $($this).val().toUpperCase();
        var Id = $($this).attr('id').split('_')[2];
        switch (val) {
            case "ALL":
                $("#chk_IASValue1_" + Id).addClass('hidden');
                $("#chk_IASValue2_" + Id).addClass('hidden');
                $("#Span_To_" + Id).addClass('hidden');
                break;
            case "ABOVE":
                $("#chk_IASValue1_" + Id).removeClass('hidden');
                $("#chk_IASValue2_" + Id).addClass('hidden');
                $("#Span_To_" + Id).addClass('hidden');
                break;
            case "BELOW":
                $("#chk_IASValue1_" + Id).removeClass('hidden');
                $("#chk_IASValue2_" + Id).addClass('hidden');
                $("#Span_To_" + Id).addClass('hidden');
                break;
            case "BETWEEN":
                $("#chk_IASValue1_" + Id).removeClass('hidden');
                $("#chk_IASValue2_" + Id).removeClass('hidden');
                $("#Span_To_" + Id).removeClass('hidden');
                break;
        }
    }

    _PChaseCommon.prototype.CriteriaPADDL = function ($this) {
        var val = $($this).val().toUpperCase();
        var Id = $($this).attr('id').split('_')[2];
        switch (val) {
            case "ALL":
                $("#chk_PASValue1_" + Id).addClass('hidden');
                $("#chk_PASValue2_" + Id).addClass('hidden');
                $("#Span_To_" + Id).addClass('hidden');
                break;
            case "ABOVE":
                $("#chk_PASValue1_" + Id).removeClass('hidden');
                $("#chk_PASValue2_" + Id).addClass('hidden');
                $("#Span_To_" + Id).addClass('hidden');
                break;
            case "BELOW":
                $("#chk_PASValue1_" + Id).removeClass('hidden');
                $("#chk_PASValue2_" + Id).addClass('hidden');
                $("#Span_To_" + Id).addClass('hidden');
                break;
            case "BETWEEN":
                $("#chk_PASValue1_" + Id).removeClass('hidden');
                $("#chk_PASValue2_" + Id).removeClass('hidden');
                $("#Span_To_" + Id).removeClass('hidden');
                break;
        }
    }

    module._PChaseCommon = new _PChaseCommon();
    module._PChaseCommon.init();
})(PChaseModule || window.PChaseModule);

$('.textboxnotallowspace').keypress(function (e) {
    if (e.which === 32 && e.target.selectionStart === 0)
        return false;
});

$(".textboxnotallowpaste").bind("paste", function (e) {
    e.preventDefault();
});

$('.textboxallownumeric').keyup(function () {
    if (/\D/g.test(this.value)) {
        // Filter non-digits from input value.
        this.value = this.value.replace(/\D/g, '');
    }
});

$('.textboxallowusphone').keypress(function (e) {
    var key = e.charCode || e.keyCode || 0;
    var phone = $(this);
    if (phone.val().length === 0) {
        phone.val(phone.val() + '(');
    }
    // Auto-format- do not expose the mask as the user begins to type
    if (key !== 8 && key !== 9) {
        if (phone.val().length === 4) {
            phone.val(phone.val() + ')');
        }
        if (phone.val().length === 5) {
            phone.val(phone.val() + ' ');
        }
        if (phone.val().length === 9) {
            phone.val(phone.val() + '-');
        }
        if (phone.val().length >= 14) {
            phone.val(phone.val().slice(0, 13));
        }
    }

    // Allow numeric (and tab, backspace, delete) keys only
    return (key == 8 ||
        key == 9 ||
        key == 46 ||
        (key >= 48 && key <= 57) ||
        (key >= 96 && key <= 105));
}).on('focus', function () {
    phone = $(this);

    if (phone.val().length === 0) {
        phone.val('(');
    } else {
        var val = phone.val();
        phone.val('').val(val); // Ensure cursor remains at the end
    }
}).on('blur', function () {
    $phone = $(this);

    if ($phone.val() === '(') {
        $phone.val('');
    }
});

$('.textboxallowchars').keypress(function (e) {
    var regex = new RegExp("^[0-9A-Za-z_.-]+$");
    var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    $('.textboxallowchars').empty().append('');
    if (e.which === 46 && $(this).val().indexOf('.') !== -1) {
        e.preventDefault();
        $('.textboxallowchars').empty().append('Please enter dot one time');
        return false;
    }
    else if (regex.test(str)) {
        $('.textboxallowchars').empty().append('');
        return true;
    }
    else {
        e.preventDefault();
        $('.textboxallowchars').empty().append('Please enter alphabate and dot');
        return false;
    }
});

$('body').on('click', 'a.portal_icon', function (e) {
    $('a.portal_icon.active').removeClass('active');
    $(this).addClass('active');
    localStorage.setItem("hdnAppCode", $('a.portal_icon.active').attr('data-appcode'));
    sessionStorage.setItem("CPortalAppCode", $('a.portal_icon.active').attr('data-appcode'));
});

if ($("#hdnIsGrouping").val() === 'Yes') {
    function updateCount() {
        var total = $('#company-dropdown-menu-render .company-checkbox').length;
        var checked = $('#company-dropdown-menu-render .company-checkbox:checked').length;
        $('#company-dropdown-toggle-render').html('Selected ' + checked + ' of ' + total + ' <span class="downCaret"></span>');
        //$("#ScrollMoreCompanyListWrapper").perfectScrollbar("destroy");
        //$("#ScrollMoreCompanyListWrapper").perfectScrollbar({ minScrollbarLength: 50 });
    }

    // Toggle dropdown show/hide
    $('#company-dropdown-toggle-render').on('click', function (e) {
        $($(e.target).find('.downCaret').toggleClass('openCaret'));
        e.stopPropagation();
        $('#company-dropdown-menu-render').toggle();
        $('#company-dropdown-menu-render .search-box').focus();
        $('#company-dropdown-menu-render .search-box').val('').trigger('keyup');
    });

    $(document).click(function () {
        $('.downCaret').removeClass('openCaret');
    });

    // Prevent closing on click inside and update count on checkbox click
    $('#company-dropdown-menu-render').on('click', function (e) {
        e.stopPropagation();
    });

    $('#company-dropdown-menu-render').on('change', 'input[type=checkbox]', function (e) {
        // Parent/child logic
        var $menu = $(this).closest('#company-dropdown-menu-render');
        var $group = $(this).closest('.group');
        if ($(this).hasClass('group-checkbox')) {           
            $group.find('.company-checkbox').prop('checked', $(this).is(':checked')).first().change();
        } else if ($(this).hasClass('company-checkbox')) {
            var allChecked = $group.find('.company-checkbox').length === $group.find('.company-checkbox:checked').length;
            $group.find('.group-checkbox').prop('checked', allChecked);
        }
        updateCount();
    });

    // Hide on outside click
    $(document).on('click', function () {
        $('#company-dropdown-menu-render').hide();
    });

    // SEARCH functionality
    $('#company-dropdown-menu-render .search-box').on('keyup', function () {
        var val = $(this).val().toLowerCase();
        $('#company-dropdown-menu-render .group,#company-dropdown-menu-render .companies').each(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(val) > -1)
        });
        if (!PChaseModule._PChaseCommon.isNullOrEmpty(val)) {
            $("#ScrollMoreCompanyListWrapper").scrollTop(0);
        } else {
            $('.childGroupCompany input[type="checkbox"]').each(function () {                
                if (this.checked) {
                    var topOffset = $('#ScrollMoreCompanyListWrapper').offset().top;
                    var parentOffset = $(this).offset().top;
                    //var parentOffset = $(this).closest('div.parentGroupCompany').offset().top;
                    var distance = parentOffset - topOffset - 100;
                    if (distance > 1) {
                        $("#ScrollMoreCompanyListWrapper").scrollTop(distance);
                    }
                }
            });
        }
    });

    // Initial label
    updateCount();

    
}