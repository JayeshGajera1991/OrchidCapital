(function (module) {
    "use strict";
    const _configs = module.configs,
        _root = '#common-aportal-wrapper';

    // Create root
    let _APortalCustomers = function () {
        this._SelectedCode = '';
        this._pageIndex = 0;
        this._pageSize = _configs.defaultPageSize;
        this.defaultPageSize = _configs.defaultPageSize;
        this._totalRecords = 0;
        this._filter = '';
        this._active = 2;
        this._sortColumn = 'UserName';
        this._sortDirection = 'ASC';
    };

    // Page Load After Call This Function
    _APortalCustomers.prototype.init = function () {
        module._APortalCustomers.load();
    }

    _APortalCustomers.prototype.load = function () {
        APortalCustomers._APortalCustomers.List();
    }

    _APortalCustomers.prototype.List = function () {
        let url = module.urls.list;
        let data = {
            PageNumber: this._pageIndex,
            PageSize: this._pageSize,
            Filter: this._filter,
            IsActive: this._active,
            SortColumn: this._sortColumn,
            SortDirection: this._sortDirection,
            SPName: 'APortal_CustomerManage'
        };
        APortalModule._APortalAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
            if (APortalCustomers._APortalCustomers._pageIndex > 0) {
                $(`${_root} #customers-tbody-render`).append(response);
            } else if (APortalCustomers._APortalCustomers._pageIndex == 0) {
                $(`${_root} #customers-tbody-render`).empty();
                $(`${_root} #customers-tbody-render`).html(response);
            }
            var PageTotal = $(`${_root} #tblCustomersList tbody tr:first`).attr('data-total-records') || 0;
            var PageIndex = PageTotal == 0 ? 0 : 1;
            var PageSize = PageTotal > $(`${_root} #tblCustomersList tbody tr`).length ? $(`${_root} #tblCustomersList tbody tr`).length : PageTotal;
            APortalCustomers._APortalCustomers._pageIndex = $(`${_root} #tblCustomersList tbody tr`).length;
            APortalCustomers._APortalCustomers._totalRecords = parseInt($(`${_root} #tblCustomersList tbody tr:first`).attr('data-total-records') || 0);
            document.getElementById("pagination").innerText =
                `Showing ${PageIndex} to ${PageSize} of ${PageTotal}`;
            resizeTableHtml();
        });
    }

    _APortalCustomers.prototype.Create = function () {
        let url = module.urls.create;
        let data = { SPName: 'APortal_CustomerManage' };
        APortalModule._APortalAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
            $(`${_root} #add_customer_form_wrapper`).empty().html(response);
            if (parseInt($("#Id").val()) > 0) {
                document.getElementById('nav_customer_form').reset();
                document.querySelectorAll('#nav_customer_form input, #nav_customer_form textarea').forEach(el => {
                    el.value = '';
                });
            }
            $(function () {
                $("#DOB").datepicker({
                    dateFormat: "yy-mm-dd",
                    changeMonth: true,
                    changeYear: true,
                    yearRange: "1900:2500"
                });
            });
            //$(window).on("load resize", resizeLayout);
            $(document).ready(function () {
                $('.select2').select2({
                    placeholder: "-- Select --",
                    allowClear: true,
                    width: '100%'
                });
            });
            $(`${_root} #h3_addCustomerHeader`).empty().html('Add New Customer Details');
            $(`${_root} #Customers-Master-Panel`).toggleClass('show');
        });
    }

    _APortalCustomers.prototype.SubmitFormData = function (event, form) {
        $("#nav_customer_form").removeData("validator");
        $("#nav_customer_form").removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse($("#nav_customer_form"));
        event.preventDefault();
        if ($("#nav_customer_form").valid()) {
            APortalModule._APortalAjaxResponse.PostValidationAjaxJsonResponse(form, function (response) {
                if (response.isSuccess) {
                    document.getElementById('nav_customer_form').reset();
                    document.querySelectorAll('#nav_customer_form input, #nav_customer_form textarea').forEach(el => {
                        el.value = '';
                    });
                    $(`${_root} #Customers-Master-Panel`).removeClass('show');
                    APortalModule._APortalToaster._toastr(1, response.message);
                    APortalCustomers._APortalCustomers._pageIndex = 0;
                    APortalCustomers._APortalCustomers.List();
                } else {
                    APortalModule._APortalToaster._toastr(0, response.message);
                }
            });
        }
    }

    _APortalCustomers.prototype.Edit = function (code) {
        let url = module.urls.get;
        let data = { Id: code, SPName: 'APortal_CustomerManage' };
        APortalModule._APortalAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
            $(`${_root} #add_customer_form_wrapper`).empty().html(response);
            $(`${_root} #Customers-Master-Panel`).addClass('show');
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
            //$(window).on("load resize", resizeLayout);
            $(document).ready(function () {
                $('.select2').select2({
                    placeholder: "-- Select --",
                    allowClear: true,
                    width: '100%'
                });
            });
            $(`${_root} #h3_addCustomerHeader`).empty().html('Edit Customer Details');
        });
    };

    _APortalCustomers.prototype.Status = function (code, status) {
        let url = module.urls.status;
        let data = { Id: code, SPName: 'APortal_CustomerManage', IsActive: status === "True" ? false : true };
        APortalModule._APortalAjaxResponse.GetAjaxJsonResponse(url, data, function (response) {
            if (response.isSuccess) {
                document.querySelectorAll(".action-menu").forEach(menu => {
                    menu.style.display = "none";
                });
                APortalModule._APortalToaster._toastr(1, response.message);
                APortalCustomers._APortalCustomers._pageIndex = 0;
                APortalCustomers._APortalCustomers.List();
            }
            else {
                APortalModule._APortalToaster._toastr(0, response.message);
            }
        });
    };

    _APortalCustomers.prototype.DeleteUser = function (code) {
        let url = module.urls.deleteUser;
        let data = { SelUserName: code };
        APortalModule._APortalAjaxResponse.PostAjaxJsonResponse(url, data, function (response) {
            if (response.isSuccess) {
                document.querySelectorAll(".action-menu").forEach(menu => {
                    menu.style.display = "none";
                });
                APortalModule._APortalToaster._toastr(1, response.message);
                APortalCustomers._APortalCustomers._pageIndex = 0;
                APortalCustomers._APortalCustomers.List();
            } else {
                APortalModule._APortalToaster._toastr(0, response.message);
            }
        });
    };

    _APortalCustomers.prototype.SendWelcomeEmailUser = function (code) {
        let url = module.urls.sendWelcomeEmailUser;
        let data = { SelUserName: code };
        APortalModule._APortalAjaxResponse.PostAjaxJsonResponse(url, data, function (response) {
            if (response.isSuccess) {
                document.querySelectorAll(".action-menu").forEach(menu => {
                    menu.style.display = "none";
                });
                APortalModule._APortalToaster._toastr(1, response.message);
                APortalCustomers._APortalCustomers._pageIndex = 0;
                APortalCustomers._APortalCustomers.List();
            } else {
                APortalModule._APortalToaster._toastr(0, response.message);
            }
        });
    };

    _APortalCustomers.prototype.UnlockUser = function (code) {
        let url = module.urls.unlockUser;
        let data = { SelUserName: code };
        APortalModule._APortalAjaxResponse.PostAjaxJsonResponse(url, data, function (response) {
            if (response.isSuccess) {
                document.querySelectorAll(".action-menu").forEach(menu => {
                    menu.style.display = "none";
                });
                APortalModule._APortalToaster._toastr(1, response.message);
                APortalCustomers._APortalCustomers._pageIndex = 0;
                APortalCustomers._APortalCustomers.List();
            } else {
                APortalModule._APortalToaster._toastr(0, response.message);
            }
        });
    };

    _APortalCustomers.prototype.ResetPasswordUser = function (code) {
        let url = module.urls.resetPasswordUser;
        let data = { SelUserName: code };
        APortalModule._APortalAjaxResponse.PostAjaxJsonResponse(url, data, function (response) {
            if (response.isSuccess) {
                document.querySelectorAll(".action-menu").forEach(menu => {
                    menu.style.display = "none";
                });
                APortalModule._APortalToaster._toastr(1, response.message);
                APortalCustomers._APortalCustomers._pageIndex = 0;
                APortalCustomers._APortalCustomers.List();
            } else {
                APortalModule._APortalToaster._toastr(0, response.message);
            }
        });
    };

    _APortalCustomers.prototype.ResetTwoFactorAuthenticationUser = function (code) {
        let url = module.urls.resetTwoFactorAuthenticationUser;
        let data = { SelUserName: code };
        APortalModule._APortalAjaxResponse.PostAjaxJsonResponse(url, data, function (response) {
            if (response.isSuccess) {
                document.querySelectorAll(".action-menu").forEach(menu => {
                    menu.style.display = "none";
                });
                APortalModule._APortalToaster._toastr(1, response.message);
                APortalCustomers._APortalCustomers._pageIndex = 0;
                APortalCustomers._APortalCustomers.List();
            } else {
                APortalModule._APortalToaster._toastr(0, response.message);
            }
        });
    };

    _APortalCustomers.prototype.ExportToExcel = function (SPName, title) {
        let url = module.urls.export;
        let data = {
            Filter: this._filter,
            IsActive: this._active,
            SortColumn: this._sortColumn,
            SortDirection: this._sortDirection,
            SPName: SPName
        };
        APortalModule._APortalAjaxResponse.DownloadFile(url, data, title, function (response) {
            if (response.isSuccess) {
                // Handle successful export
            }
            else {
                APortalModule._APortalToaster._toastr(0, response.message);
            }
        });
    };

    _APortalCustomers.prototype.LoadCities = function (state, cityDropdownId) {
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

    _APortalCustomers.prototype.toggleMenu = function (btn) {
        const menu = btn.nextElementSibling;

        // close other menus first
        document.querySelectorAll(".action-menu").forEach(m => {
            if (m !== menu) m.style.display = "none";
        });

        // toggle current menu
        menu.style.display = (menu.style.display === "block") ? "none" : "block";

        // close on outside click
        const handler = (e) => {
            if (!btn.parentElement.contains(e.target)) {
                menu.style.display = "none";
                document.removeEventListener("click", handler);
            }
        };

        document.addEventListener("click", handler);
    };

    module._APortalCustomers = new _APortalCustomers();
    module._APortalCustomers.init();
})(APortalCustomers || window.APortalCustomers);