(function (module) {
    "use strict";
    const _configs = module.configs,
        _root = '#common-aportal-wrapper';

    // Create root
    let _APortalTwoFactor = function () {
        this._IsTwoFctor = false;
    };

    // Page Load After Call This Function
    _APortalTwoFactor.prototype.init = function () {
        module._APortalTwoFactor.load();
    }

    _APortalTwoFactor.prototype.load = function () {
        $(document).ready(function () {
            $('.select2').select2({
                placeholder: "-- Select --",
                allowClear: true,
                width: '100%'
            });
        });
        //APortalTwoFactor._APortalTwoFactor.List();
    }

    _APortalTwoFactor.prototype.EnableTwoFactor = function ($this) {
        if ($($this).is(':checked')) {
            let url = module.urls.twofactor;
            let data = '{}';
            APortalModule._APortalAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
                $(`${_root} #modalBodyTwoFactor`).empty().html(response);
                $(`${_root} #TwoFactorModal`).addClass('show');
            });
        } else {
            let url = module.urls.disablestwofactor;
            let data = '{}';
            APortalModule._APortalAjaxResponse.GetAjaxJsonResponse(url, data, function (response) {
                if (response.isSuccess) {
                    $(`${_root} #TwoFactorModal`).removeClass('show');
                    $("#modalBodyTwoFactor").empty();
                    APortalModule._APortalToaster._toastr(1, response.message);
                    window.location.href = module.urls.profile;
                } else {
                    APortalModule._APortalToaster._toastr(0, response.message);
                }
            });
        }
    }

    _APortalTwoFactor.prototype.closeModal = function () {
        $(`${_root} #TwoFactorModal`).removeClass('show');
        $("#modalBodyTwoFactor").empty();
        $(`${_root} #UpdateUserProfilerModal`).removeClass('show');
        $("#modalBodyUserProfiler").empty();
    };

    _APortalTwoFactor.prototype.GenerateQRCode = function (step) {
        if ($(`${_root} input[name="device"]:checked`).length === 0) {
            alert("Please select a device type.");
            return;
        }
        let url = module.urls.getqrcode;
        let data = '{}';
        APortalModule._APortalAjaxResponse.GetAjaxJsonResponse(url, data, function (response) {
            if (response.qrCodeUrl != null || response.qrCodeUrl != "") {
                // Base64 QR Image
                var qrImage = response.qrCodeUrl;
                $(`#imgQRCode`).attr('src', qrImage);
                document.querySelectorAll(".step").forEach(s => s.classList.remove("active"));
                document.getElementById("step" + step).classList.add("active");
            }
        });
    }

    _APortalTwoFactor.prototype.VerificationCode = function (step) {
        $(`${_root} #txtVerificationCode`).val('');
        document.querySelectorAll(".step").forEach(s => s.classList.remove("active"));
        document.getElementById("step" + step).classList.add("active");
    }

    _APortalTwoFactor.prototype.SubmitFormData = function (event, form) {
        if ($(`${_root} #txtVerificationCode`).val().length === 0) {
            alert("Please enter the verification code.");
            event.preventDefault();
            return;
        }
        $("#nav_twofactor_form").removeData("validator");
        $("#nav_twofactor_form").removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse($("#nav_twofactor_form"));
        event.preventDefault();
        if ($("#nav_twofactor_form").valid()) {
            APortalModule._APortalAjaxResponse.PostValidationAjaxJsonResponse(form, function (response) {
                if (response.isSuccess) {
                    document.getElementById('nav_twofactor_form').reset();
                    document.querySelectorAll('#nav_twofactor_form input, #nav_twofactor_form textarea').forEach(el => {
                        el.value = '';
                    });
                    APortalModule._APortalToaster._toastr(1, response.message);
                    $(`${_root} #TwoFactorModal`).removeClass('show');
                    $("#modalBodyTwoFactor").empty();
                    window.location = module.urls.profile;
                } else {
                    APortalModule._APortalToaster._toastr(0, response.message);
                }
            });
        }

    };

    _APortalTwoFactor.prototype.Status = function (code, status) {
        let url = module.urls.status;
        let data = { Id: code, SPName: 'APortal_PageMst', IsActive: status === "True" ? false : true };
        APortalModule._APortalAjaxResponse.GetAjaxJsonResponse(url, data, function (response) {
            if (response.isSuccess) {
                document.querySelectorAll(".action-menu").forEach(menu => {
                    menu.style.display = "none";
                });
                APortalModule._APortalToaster._toastr(1, response.message);
                APortalTwoFactor._APortalTwoFactor._pageIndex = 0;
                APortalTwoFactor._APortalTwoFactor.List();
            } else {
                APortalModule._APortalToaster._toastr(0, response.message);
            }
        });
    };

    _APortalTwoFactor.prototype.UpdateProfileImage = function ($this) {
        let file = $this.files[0];
        if (file) {
            let reader = new FileReader();
            reader.onload = function (e) {
                $('#preview').attr('src', e.target.result);
            }
            reader.readAsDataURL(file);
        }
        let url = module.urls.updateprofileimage;
        var formData = new FormData();
        formData.append('ImageFile', file);
        formData.append('ImageUrl', module.configs.loginUserImage);
        APortalModule._APortalAjaxResponse.PostFromAjaxJsonResponse(url, formData, function (response) {
            if (response.isSuccess) {
                APortalModule._APortalToaster._toastr(1, response.message);
                $("#preview").attr("src", `~/ImageProfiler/${response.imageUrl}`);
                $(`#img_ImageProfiler`).attr("src", `~/ImageProfiler/${response.imageUrl}`);
                window.location.reload();
            } else {
                APortalModule._APortalToaster._toastr(0, response.message);
            }
        });
    }

    _APortalTwoFactor.prototype.OpenUpdateUserProfileModal = function (code) {
        let url = module.urls.get;
        let data = { Id: code, SPName: 'APortal_UserManage' };
        APortalModule._APortalAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
            $(`${_root} #modalBodyUserProfiler`).empty().html(response);
            $(`${_root} #UpdateUserProfilerModal`).addClass('show');
            $("#State").change();
            document.querySelectorAll(".action-menu").forEach(menu => {
                menu.style.display = "none";
            });
            $(function () {
                $("#DOB").datepicker({
                    dateFormat: "yy-mm-dd",
                    changeMonth: true,
                    changeYear: true,
                    yearRange: "1900:2500"
                });
            });
            $(document).ready(function () {
                $('.select2').select2({
                    placeholder: "-- Select --",
                    allowClear: true,
                    width: '100%'
                });
            });
        });
    };

    _APortalTwoFactor.prototype.SubmitFormData = function (event, form) {
        $("#nav_agentuser_form").removeData("validator");
        $("#nav_agentuser_form").removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse($("#nav_agentuser_form"));
        event.preventDefault();
        if ($("#nav_agentuser_form").valid()) {
            APortalModule._APortalAjaxResponse.PostValidationAjaxJsonResponse(form, function (response) {
                if (response.isSuccess) {
                    document.getElementById('nav_agentuser_form').reset();
                    document.querySelectorAll('#nav_agentuser_form input, #nav_agentuser_form textarea').forEach(el => {
                        el.value = '';
                    });
                    $(`${_root} #UpdateUserProfilerModal`).removeClass('show');
                    $("#modalBodyUserProfiler").empty();
                    APortalModule._APortalToaster._toastr(1, response.message);
                    window.location.reload();
                } else {
                    APortalModule._APortalToaster._toastr(0, response.message);
                }
            });
        }
    }

    _APortalTwoFactor.prototype.LoadCities = function (state, cityDropdownId) {
        let url = module.urls.cities;
        let data = { state: state };
        APortalModule._APortalAjaxResponse.GetAjaxJsonResponse(url, data, function (response) {
            let cityDropdown = document.getElementById(cityDropdownId);
            cityDropdown.innerHTML = "";
            response.forEach(city => {
                let option = document.createElement("option");
                option.value = city;
                option.text = city;
                cityDropdown.appendChild(option);
            });
        });
    };

    _APortalTwoFactor.prototype.DeleteUserProfileImage = function (userName) {
        let url = module.urls.deleteProfileImage;
        let data = { UserName: userName };
        APortalModule._APortalAjaxResponse.PostAjaxJsonResponse(url, data, function (response) {
            if (response.isSuccess) {
                APortalModule._APortalToaster._toastr(1, response.message);
                window.location.reload();
            } else {
                APortalModule._APortalToaster._toastr(0, response.message);
            }
        });
    };

    module._APortalTwoFactor = new _APortalTwoFactor();
    module._APortalTwoFactor.init();
})(APortalTwoFactor || window.APortalTwoFactor);