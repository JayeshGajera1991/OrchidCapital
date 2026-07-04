(function (module) {
    "use strict";
    const _configs = module.configs,
        _root = '#common-aportal-wrapper';

    // Create root
    let _APortalLoader = function () { };

    _APortalLoader.prototype.init = function () {
        module._APortalLoader.load();
    }

    _APortalLoader.prototype.load = function () {

    }

    _APortalLoader.prototype.LoaderStart = function () {
        $(`${_root} .loader`).css('display', 'block');
    }

    _APortalLoader.prototype.LoaderEnd = function () {
        $(`${_root} .loader`).css('display', 'none');
    }
    module._APortalLoader = new _APortalLoader();
    module._APortalLoader.init();
})(APortalModule || window.APortalModule);