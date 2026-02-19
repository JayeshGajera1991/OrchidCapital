(function (module) {
    "use strict";
    const _configs = module.configs,
        _root = '#common-cportallogin-wrapper',
        toastrTimeout = 3000;

    // Create root
    let _PChanseToaster = function () { };

    _PChanseToaster.prototype.init = function () {
        module._PChanseToaster.load();
    }

    _PChanseToaster.prototype.load = function () {

    }

    _PChanseToaster.prototype._toastr = function (type, message) {
        let ele = type === 1 ? '#toast-common-success' : type === 2 ? '#toast-common-warning' : type === 3 ? '#toast-common-info' : '#toast-common-error';
        $(ele).addClass('show').css('display','flex');
        $(ele).find("#toast-content").empty().html(message);

        setTimeout(function () {
            $(ele).removeClass('show').css('display', 'none');
        }, toastrTimeout);
    }

    _PChanseToaster.prototype._failure = function (response) {
        let msg = response.responseJSON?.Message || PChaseModule._PChanseToaster._isJson(response.responseText) || response.statusText || response.message;
        PChaseModule._PChanseToaster._toastr(0, msg);
        PChaseModule._PChaseLoader.LoaderEnd();
    }

    _PChanseToaster.prototype._error = function (response) {
        PChaseModule._PChaseLoader.LoaderEnd();
        //if (response.status === 401) {
        //    window.location.reload();
        //    return;
        //} else {
        if (response !== undefined) {
            let msg = response.responseJSON?.Message || PChaseModule._PChanseToaster._isJson(response.responseText) || response.statusText || response.message;
            PChaseModule._PChanseToaster._toastr(0, msg);
        }        
        /*}*/
    }

    _PChanseToaster.prototype._isJson = function (json) {
        try {
            let d = JSON.parse(json);
            return d.Message;
        } catch (e) {
            console.log(e);
            return null;
        }
    },

        module._PChanseToaster = new _PChanseToaster();
    module._PChanseToaster.init();
})(PChaseModule || window.PChaseModule);