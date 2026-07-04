(function (module) {
    "use strict";
    const _configs = module.configs,
        _root = '#common-aportal-wrapper',
        toastrTimeout = 3000;

    // Create root
    let _APortalToaster = function () { };

    _APortalToaster.prototype.init = function () {
        module._APortalToaster.load();
    }

    _APortalToaster.prototype.load = function () {

    }

    _APortalToaster.prototype._toastr = function (type, message) {
        if (type === 1) {
            toastr.success(message);
        } else if (type === 0) {
            toastr.error(message);
        } else if (type === 2) {
            toastr.warning(message);
        } else if (type === 3) {
            toastr.info(message);
        }
    }

    _APortalToaster.prototype._failure = function (response) {
        let msg = response.responseJSON?.Message || APortalModule._APortalToaster._isJson(response.responseText) || response.statusText || response.message;
        toastr.error(msg);
        APortalModule._APortalLoader.LoaderEnd();
    }

    _APortalToaster.prototype._error = function (response) {
        APortalModule._APortalLoader.LoaderEnd();
        //if (response.status === 401) {
        //    window.location.reload();
        //    return;
        //} else {
        if (response !== undefined) {
            let msg = response.responseJSON?.Message || APortalModule._APortalToaster._isJson(response.responseText) || response.statusText || response.message;
            toastr.error(msg);
        }
        /*}*/
    }

    _APortalToaster.prototype._isJson = function (json) {
        try {
            let d = JSON.parse(json);
            return d.Message;
        } catch (e) {
            console.log(e);
            return null;
        }
    }

    module._APortalToaster = new _APortalToaster();
    module._APortalToaster.init();
})(APortalModule || window.APortalModule);