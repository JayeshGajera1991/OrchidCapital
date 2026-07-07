(function (module) {
    "use strict";
    const _configs = module.configs,
        _root = '#common-aportal-wrapper';

    // Create root
    let _APortalTeams = function () {
        this._SelectedCode = '';
        this._pageIndex = 0;
        this._pageSize = _configs.defaultPageSize;
        this.defaultPageSize = _configs.defaultPageSize;
        this._totalRecords = 0;
        this._filter = '';
        this._active = 2;
        this._sortColumn = 'FullName';
        this._sortDirection = 'ASC';
    };

    // Page Load After Call This Function
    _APortalTeams.prototype.init = function () {
        module._APortalTeams.load();
    }

    _APortalTeams.prototype.load = function () {
        $(document).ready(function () {
            $('.select2').select2({
                placeholder: "-- Select --",
                allowClear: true,
                width: '100%'
            });
        });
        APortalTeams._APortalTeams.List();
    }

    _APortalTeams.prototype.List = function () {
        let url = module.urls.list;
        let data = {
            PageNumber: this._pageIndex,
            PageSize: this._pageSize,
            Filter: this._filter,
            IsActive: this._active,
            SortColumn: this._sortColumn,
            SortDirection: this._sortDirection,
            SPName: 'APortal_OrchidCapitalTeamMst'
        };
        APortalModule._APortalAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
            if (APortalTeams._APortalTeams._pageIndex > 0) {
                $(`${_root} #teams-tbody-render`).append(response);
            } else if (APortalTeams._APortalTeams._pageIndex == 0) {
                $(`${_root} #teams-tbody-render`).empty();
                $(`${_root} #teams-tbody-render`).html(response);
            }
            var PageTotal = $(`${_root} #tblTeamsList tbody tr:first`).attr('data-total-records') || 0;
            var PageIndex = PageTotal == 0 ? 0 : 1;
            var PageSize = PageTotal > $(`${_root} #tblTeamsList tbody tr`).length ? $(`${_root} #tblTeamsList tbody tr`).length : PageTotal;
            APortalTeams._APortalTeams._pageIndex = $(`${_root} #tblTeamsList tbody tr`).length;
            APortalTeams._APortalTeams._totalRecords = parseInt($(`${_root} #tblTeamsList tbody tr:first`).attr('data-total-records') || 0);
            document.getElementById("pagination").innerText =
                `Showing ${PageIndex} to ${PageSize} of ${PageTotal}`;
            resizeTableHtml();
        });
    }

    _APortalTeams.prototype.Create = function () {
        let url = module.urls.create;
        let data = { SPName: 'APortal_OrchidCapitalTeamMst' };
        APortalModule._APortalAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
            $(`${_root} #add_teams_form_wrapper`).empty().html(response);
            if (parseInt($("#Id").val()) > 0) {
                document.getElementById('nav_teams_form').reset();
                document.querySelectorAll('#nav_teams_form input, #nav_teams_form textarea').forEach(el => {
                    el.value = '';
                });
            }
            $(`${_root} #h3_addTeamHeader`).empty().html('Add New Team Details');
            $(`${_root} #Teams-Master-Panel`).toggleClass('show');
        });
    }

    _APortalTeams.prototype.SubmitFormData = function (event, form) {
        $("#nav_teams_form").removeData("validator");
        $("#nav_teams_form").removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse($("#nav_teams_form"));
        event.preventDefault();
        if ($("#nav_teams_form").valid()) {
            APortalModule._APortalAjaxResponse.PostValidationAjaxJsonResponse(form, function (response) {
                if (response.isSuccess) {
                    document.getElementById('nav_teams_form').reset();
                    document.querySelectorAll('#nav_teams_form input, #nav_teams_form textarea').forEach(el => {
                        el.value = '';
                    });
                    $(`${_root} #Teams-Master-Panel`).removeClass('show');
                    APortalTeams._APortalTeams._pageIndex = 0;
                    APortalTeams._APortalTeams.List();
                }
            });
        }
    }

    _APortalTeams.prototype.Edit = function (code) {
        let url = module.urls.get;
        let data = { Id: code, SPName: 'APortal_OrchidCapitalTeamMst' };
        APortalModule._APortalAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
            $(`${_root} #add_teams_form_wrapper`).empty().html(response);
            $(`${_root} #h3_addTeamHeader`).empty().html('Edit Team Details');
            $(`${_root} #Teams-Master-Panel`).addClass('show');
            document.querySelectorAll(".action-menu").forEach(menu => {
                menu.style.display = "none";
            });
        });
    };

    _APortalTeams.prototype.Status = function (code, status) {
        let url = module.urls.status;
        let data = { Id: code, SPName: 'APortal_OrchidCapitalTeamMst', IsActive: status === "True" ? false : true };
        APortalModule._APortalAjaxResponse.GetAjaxJsonResponse(url, data, function (response) {
            if (response.isSuccess) {
                document.querySelectorAll(".action-menu").forEach(menu => {
                    menu.style.display = "none";
                });
                APortalTeams._APortalTeams._pageIndex = 0;
                APortalTeams._APortalTeams.List();
            }
        });
    };

    _APortalTeams.prototype.Delete = function (code, status) {
        let url = module.urls.delete;
        let data = { Id: code, SPName: 'APortal_OrchidCapitalTeamMst' };
        APortalModule._APortalAjaxResponse.GetAjaxJsonResponse(url, data, function (response) {
            if (response.isSuccess) {
                document.querySelectorAll(".action-menu").forEach(menu => {
                    menu.style.display = "none";
                });
                APortalTeams._APortalTeams._pageIndex = 0;
                APortalTeams._APortalTeams.List();
            }
        });
    };

    _APortalTeams.prototype.ExportToExcel = function (SPName, title) {
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
        });
    };

    _APortalTeams.prototype.toggleMenu = function (btn) {
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

    module._APortalTeams = new _APortalTeams();
    module._APortalTeams.init();
})(APortalTeams || window.APortalTeams);