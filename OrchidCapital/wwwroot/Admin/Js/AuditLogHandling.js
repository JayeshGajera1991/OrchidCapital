(function (module) {
    "use strict";
    const _configs = module.configs,
        _root = '#common-aportal-wrapper';

    // Create root
    let _APortalAuditLogHandling = function () {
        this._SelectedCode = '';
        this._pageIndex = 1;
        this._pageSize = _configs.defaultPageSize;
        this.defaultPageSize = _configs.defaultPageSize;
        this._totalRecords = 0;
        this._filter = '';
        this._active = 2;
        this._sortColumn = 'CreatedUser';
        this._sortDirection = 'ASC';
    };

    // Page Load After Call This Function
    _APortalAuditLogHandling.prototype.init = function () {
        module._APortalAuditLogHandling.load();
    }

    _APortalAuditLogHandling.prototype.load = function () {
        APortalAuditLogHandling._APortalAuditLogHandling.List();
    }

    _APortalAuditLogHandling.prototype.List = function () {
        let url = module.urls.list;
        let data = {
            PageNumber: this._pageIndex,
            PageSize: this._pageSize,
            Filter: this._filter,
            IsActive: this._active,
            SortColumn: this._sortColumn,
            SortDirection: this._sortDirection,
            SPName: 'UPortal_uspSaveAuditLog'
        };
        APortalModule._APortalAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
            if (APortalAuditLogHandling._APortalAuditLogHandling._pageIndex > 0) {
                $(`${_root} #auditloghandling-tbody-render`).append(response);
            } else if (APortalAuditLogHandling._APortalAuditLogHandling._pageIndex == 0) {
                $(`${_root} #auditloghandling-tbody-render`).empty();
                $(`${_root} #auditloghandling-tbody-render`).html(response);
            }
            var PageTotal = $(`${_root} #tblAuditLogHandlingList tbody tr:first`).attr('data-total-records') || 0;
            var PageIndex = PageTotal == 0 ? 0 : 1;;
            var PageSize = PageTotal > $(`${_root} #tblAuditLogHandlingList tbody tr`).length ? $(`${_root} #tblAuditLogHandlingList tbody tr`).length : PageTotal;
            APortalAuditLogHandling._APortalAuditLogHandling._pageIndex = $(`${_root} #tblAuditLogHandlingList tbody tr`).length;
            APortalAuditLogHandling._APortalAuditLogHandling._totalRecords = parseInt($(`${_root} #tblAuditLogHandlingList tbody tr:first`).attr('data-total-records') || 0);
            document.getElementById("pagination").innerText =
                `Showing ${PageIndex} to ${PageSize} of ${PageTotal}`;
            resizeTableHtml();
        });
    }

    _APortalAuditLogHandling.prototype.ExportToExcel = function (SPName, title) {
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

    _APortalAuditLogHandling.prototype.toggleMenu = function (btn) {
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

    module._APortalAuditLogHandling = new _APortalAuditLogHandling();
    module._APortalAuditLogHandling.init();
})(APortalAuditLogHandling || window.APortalAuditLogHandling);