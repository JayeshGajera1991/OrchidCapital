var LoaderCounter = {
    count: 0,
    start: function () {
        if (this.count === 0) {
            PChaseModule._PChaseLoader.LoaderStart();
        }
        this.count++;
    },
    end: function () {
        this.count--;
        if (this.count <= 0) {
            this.count = 0;
            PChaseModule._PChaseLoader.LoaderEnd();
        }
    }
};
(function (module) {
    "use strict";
    const _configs = module.configs,
        _root = '#common-cportallogin-wrapper';

    // Create root
    let _PChaseAjaxResponse = function () { };

    // Page Load After Call This Function
    _PChaseAjaxResponse.prototype.init = function () {
        module._PChaseAjaxResponse.load();
    }

    _PChaseAjaxResponse.prototype.load = function () {

    }

    _PChaseAjaxResponse.prototype.GetAjaxHtmlResponse = function (url, data, successCallback, async = true, loader = true) {
        if (loader) LoaderCounter.start();
        $.ajax({
            url: url,
            data: data,
            type: "GET",
            contentType: "application/html; charset=utf-8",
            dataType: "html",
            beforeSend: function () {  },
            async: async,
            headers: {
                "Accept-Language": PChaseModule.configs.loginUserCultureCode,
                "App-ModuleBaseUrl": PChaseModule._PChaseCommon._GetCurrentProjectBaseUrl,
                "Access-Control-Allow-Origin": PChaseModule.configs.basePath,
                "Access-Control-Allow-Credentials": true,
                "Access-Control-Allow-Methods": 'GET,PUT,POST,DELETE',
                "Access-Control-Allow-Headers": "Origin, X-Requested-With, Content-Type, Accept"
            },
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            success: function (response) {
                if (typeof successCallback === "function") {
                    successCallback(response);
                }
            },
            error: function (response) {               
                PChaseModule._PChanseToaster._error(response);
            },
            failure: function (response) {               
                PChaseModule._PChanseToaster._failure(response);
            },
            complete: function () {
                LoaderCounter.end();
            }
        });
    }

    _PChaseAjaxResponse.prototype.GetAjaxJsonResponse = function (url, data, successCallback, async = true, loader = true) {       
        if (loader) LoaderCounter.start();
        $.ajax({
            url: url,
            data: data,
            type: "GET",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {  },
            async: async,
            headers: {
                "Accept-Language": PChaseModule.configs.loginUserCultureCode,
                "App-ModuleBaseUrl": PChaseModule._PChaseCommon._GetCurrentProjectBaseUrl,
                "Access-Control-Allow-Origin": PChaseModule.configs.basePath,
                "App-CompanyCode": PChaseModule._PChaseCommon._companyCode,
                "App-SiteCode": PChaseModule._PChaseCommon._locationCode,
                "App-CompanyRegion": PChaseModule._PChaseCommon._companyRegion,
                "App-ERPName": PChaseModule._PChaseCommon._erpName,
                "App-EnableOCR": PChaseModule._PChaseCommon._enableOCR,
                "App-NumberOfDecimal": PChaseModule._PChaseCommon._companyNumberOfDecimal,
                "Access-Control-Allow-Credentials": true,
                "Access-Control-Allow-Methods": 'GET,PUT,POST,DELETE',
                "Access-Control-Allow-Headers": "Origin, X-Requested-With, Content-Type, Accept"
            },
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            success: function (response) {
                if (typeof successCallback === "function") {
                    successCallback(response);
                }
            },
            error: function (response) {
                PChaseModule._PChanseToaster._error(response);
            },
            failure: function (response) {
                PChaseModule._PChanseToaster._failure(response);
            },
            complete: function () {
                LoaderCounter.end();
            }
        });
    }

    _PChaseAjaxResponse.prototype.PostAjaxJsonResponse = function (url, data, successCallback, async = true, loader = true) {
        if (loader) LoaderCounter.start();
        $.ajax({
            url: url,
            data: JSON.stringify(data),
            type: "POST",
            contentType: "application/json charset=utf-8",
            dataType: "json",
            async: async,
            beforeSend: function () {  },
            cache: false,
            headers: {
                "Accept-Language": PChaseModule.configs.loginUserCultureCode,
                "App-ModuleBaseUrl": PChaseModule._PChaseCommon._GetCurrentProjectBaseUrl,
                "Access-Control-Allow-Origin": PChaseModule.configs.basePath,
                "App-CompanyCode": PChaseModule._PChaseCommon._companyCode,
                "App-SiteCode": PChaseModule._PChaseCommon._locationCode,
                "App-CompanyRegion": PChaseModule._PChaseCommon._companyRegion,
                "App-ERPName": PChaseModule._PChaseCommon._erpName,
                "App-EnableOCR": PChaseModule._PChaseCommon._enableOCR,
                "App-NumberOfDecimal": PChaseModule._PChaseCommon._companyNumberOfDecimal,
                "Access-Control-Allow-Credentials": true,
                "Access-Control-Allow-Methods": 'GET,PUT,POST,DELETE',
                "Access-Control-Allow-Headers": "Origin, X-Requested-With, Content-Type, Accept"
            },
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            success: function (response) {
                if (typeof successCallback === "function") {
                    successCallback(response);
                }
            },
            error: function (response) {
                PChaseModule._PChanseToaster._error(response);
            },
            failure: function (response) {
                PChaseModule._PChanseToaster._failure(response);
            },
            complete: function () {
                
                LoaderCounter.end();
                
            }
        });
    }

    _PChaseAjaxResponse.prototype.PostFromAjaxJsonResponse = function (url, data, successCallback, async = true, loader = true) {
        if (loader) LoaderCounter.start();
        $.ajax({
            url: url,
            data: data,
            type: "POST",
            contentType: false,
            processData: false,
            async: async,
            beforeSend: function () {},
            headers: {
                "Accept-Language": PChaseModule.configs.loginUserCultureCode,
                "App-ModuleBaseUrl": PChaseModule._PChaseCommon._GetCurrentProjectBaseUrl,
                "Access-Control-Allow-Origin": PChaseModule.configs.basePath,
                "App-CompanyCode": PChaseModule._PChaseCommon._companyCode,
                "App-SiteCode": PChaseModule._PChaseCommon._locationCode,
                "App-CompanyRegion": PChaseModule._PChaseCommon._companyRegion,
                "App-ERPName": PChaseModule._PChaseCommon._erpName,
                "App-EnableOCR": PChaseModule._PChaseCommon._enableOCR,
                "App-NumberOfDecimal": PChaseModule._PChaseCommon._companyNumberOfDecimal,
                "Access-Control-Allow-Credentials": true,
                "Access-Control-Allow-Methods": 'GET,PUT,POST,DELETE',
                "Access-Control-Allow-Headers": "Origin, X-Requested-With, Content-Type, Accept"
            },
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            success: function (response) {
                if (typeof successCallback === "function") {
                    successCallback(response);
                }
            },
            error: function (response) {
                PChaseModule._PChanseToaster._error(response);
            },
            failure: function (response) {
                PChaseModule._PChanseToaster._failure(response);
            },
            complete: function () {
                
                LoaderCounter.end();
               
            }
        });
    }

    _PChaseAjaxResponse.prototype.PostValidationAjaxJsonResponse = function ($this, successCallback, async = true, loader = true) {
        var _$this = $($this);
        var frmValues = _$this.serialize();
        if (loader) LoaderCounter.start();
        $.ajax({
            type: _$this.attr('method'),
            url: _$this.attr('action'),
            data: frmValues,
            dataType: "json",
            async: async,
            beforeSend: function () { },
            headers: {
                "Accept-Language": PChaseModule.configs.loginUserCultureCode,
                "App-ModuleBaseUrl": PChaseModule._PChaseCommon._GetCurrentProjectBaseUrl,
                "Access-Control-Allow-Origin": PChaseModule.configs.basePath,
                "App-CompanyCode": PChaseModule._PChaseCommon._companyCode,
                "App-SiteCode": PChaseModule._PChaseCommon._locationCode,
                "App-CompanyRegion": PChaseModule._PChaseCommon._companyRegion,
                "App-ERPName": PChaseModule._PChaseCommon._erpName,
                "App-EnableOCR": PChaseModule._PChaseCommon._enableOCR,
                "App-NumberOfDecimal": PChaseModule._PChaseCommon._companyNumberOfDecimal,
                "Access-Control-Allow-Credentials": true,
                "Access-Control-Allow-Methods": 'GET,PUT,POST,DELETE',
                "Access-Control-Allow-Headers": "Origin, X-Requested-With, Content-Type, Accept"
            },
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            success: function (response) {
                if (typeof successCallback === "function") {
                    successCallback(response);
                }
            },
            error: function (response) {
                PChaseModule._PChanseToaster._error(response);
            },
            failure: function (response) {
                PChaseModule._PChanseToaster._failure(response);
            },
            complete: function () {
                LoaderCounter.end();
            }
        });
    }

    _PChaseAjaxResponse.prototype.PostValidationAjaxJsonResponsewithFile = function ($this, successCallback, async = true, loader = true) {
        if (loader) LoaderCounter.start();
        var _$this = $($this);
        var _form = $($this)[0];
        var frmValues = new FormData(_form);
        $.ajax({
            type: _$this.attr('method'),
            url: _$this.attr('action'),
            data: frmValues,
            contentType: false, 
            processData: false,
            beforeSend: PChaseModule._PChaseLoader.LoaderStart(),
            complete: PChaseModule._PChaseLoader.LoaderEnd(),
            headers: {
                "Accept-Language": PChaseModule.configs.loginUserCultureCode,
                "App-ModuleBaseUrl": PChaseModule._PChaseCommon._GetCurrentProjectBaseUrl,
                "Access-Control-Allow-Origin": PChaseModule.configs.basePath,
                "App-CompanyCode": PChaseModule._PChaseCommon._companyCode,
                "App-SiteCode": PChaseModule._PChaseCommon._locationCode,
                "App-CompanyRegion": PChaseModule._PChaseCommon._companyRegion,
                "App-ERPName": PChaseModule._PChaseCommon._erpName,
                "App-EnableOCR": PChaseModule._PChaseCommon._enableOCR,
                "App-NumberOfDecimal": PChaseModule._PChaseCommon._companyNumberOfDecimal,
                "Access-Control-Allow-Credentials": true,
                "Access-Control-Allow-Methods": 'GET,PUT,POST,DELETE',
                "Access-Control-Allow-Headers": "Origin, X-Requested-With, Content-Type, Accept"
            },
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            success: function (response) {
                if (typeof successCallback === "function") {
                    successCallback(response);
                }
            },
            error: function (response) {
                PChaseModule._PChanseToaster._error(response.responseText);
            },
            failure: function (response) {
                PChaseModule._PChanseToaster._failure(response.responseText);
            }
        });
    }

    _PChaseAjaxResponse.prototype.DownloadFile = function (url, data, _filename, async = true, loader = true)
    {
        if (loader) LoaderCounter.start();
        $.ajax({
            url: url,
            data: data,
            type: "POST",
            beforeSend: function () {  },
            async: async,
            headers: {
                "Accept-Language": PChaseModule.configs.loginUserCultureCode,
                "App-ModuleBaseUrl": PChaseModule._PChaseCommon._GetCurrentProjectBaseUrl,
                "Access-Control-Allow-Origin": PChaseModule.configs.basePath,
                "App-CompanyCode": PChaseModule._PChaseCommon._companyCode,
                "App-SiteCode": PChaseModule._PChaseCommon._locationCode,
                "App-CompanyRegion": PChaseModule._PChaseCommon._companyRegion,
                "App-ERPName": PChaseModule._PChaseCommon._erpName,
                "App-EnableOCR": PChaseModule._PChaseCommon._enableOCR,
                "App-NumberOfDecimal": PChaseModule._PChaseCommon._companyNumberOfDecimal,
                "Access-Control-Allow-Credentials": true,
                "Access-Control-Allow-Methods": 'GET,PUT,POST,DELETE',
                "Access-Control-Allow-Headers": "Origin, X-Requested-With, Content-Type, Accept"
            },
            xhrFields: {
                withCredentials: true,
                responseType: 'blob'
            },
            crossDomain: true,
            success: function (response, status, xhr)
            {
                const blob = new Blob([response], { type: xhr.getResponseHeader('Content-Type') });
                const url = window.URL.createObjectURL(blob);
                const a = document.createElement('a');
                a.href = url;
                a.download = _filename;
                document.body.appendChild(a);
                a.click();
                window.URL.revokeObjectURL(url);
            },
            error: function (response)
            {
                PChaseModule._PChanseToaster._error(response);
            },
            failure: function (response)
            {
                PChaseModule._PChanseToaster._failure(response);
            },
            complete: function ()
            {
                LoaderCounter.end();
            }
        });
    }

    _PChaseAjaxResponse.prototype.PostFromAjaxProgressbarJsonResponse = function (url, data, mID, successCallback, async = true, loader = true) {
        if (loader) LoaderCounter.start();
        $.ajax({
            xhr: function () {
                var xhr = new window.XMLHttpRequest();
                xhr.upload.addEventListener("progress", function (e) {
                    $("#spndel_" + mID).addClass('hidden');
                    if (e.lengthComputable) {
                        var loaded = e.loaded;
                        var total = e.total;
                        var percent = Math.round((loaded / total) * 100);

                        $("#prg_" + mID).css("width", percent + "%");
                    };
                });
                xhr.addEventListener('load', function () {
                    $("#divprg_" + mID).addClass('hidden');
                    $("#spndel_" + mID).addClass('hidden');
                });
                return xhr;
            },
            url: url,
            data: data,
            type: "POST",
            contentType: false,
            processData: false,
            async: async,
            beforeSend: function () {  },
            headers: {
                "Accept-Language": PChaseModule.configs.loginUserCultureCode,
                "App-ModuleBaseUrl": PChaseModule._PChaseCommon._GetCurrentProjectBaseUrl,
                "Access-Control-Allow-Origin": PChaseModule.configs.basePath,
                "App-CompanyCode": PChaseModule._PChaseCommon._companyCode,
                "App-SiteCode": PChaseModule._PChaseCommon._locationCode,
                "App-CompanyRegion": PChaseModule._PChaseCommon._companyRegion,
                "App-ERPName": PChaseModule._PChaseCommon._erpName,
                "App-EnableOCR": PChaseModule._PChaseCommon._enableOCR,
                "App-NumberOfDecimal": PChaseModule._PChaseCommon._companyNumberOfDecimal,
                "Access-Control-Allow-Credentials": true,
                "Access-Control-Allow-Methods": 'GET,PUT,POST,DELETE',
                "Access-Control-Allow-Headers": "Origin, X-Requested-With, Content-Type, Accept"
            },
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            success: function (response) {
                if (typeof successCallback === "function") {
                    successCallback(response);
                }
            },
            error: function (response) {
                PChaseModule._PChanseToaster._error(response);
            },
            failure: function (response) {
                PChaseModule._PChanseToaster._failure(response);
            },
            complete: function () {
                LoaderCounter.end();
            }
        });
    }

    _PChaseAjaxResponse.prototype.CustomAjaxRequest = function (url, data, type, contentType, dataType, successCallback, async = true, loader = true) {
        if (loader) LoaderCounter.start();
        $.ajax({
            url: url,
            data: data,
            type: type,
            contentType: contentType,
            dataType: dataType,
            async: async,
            headers: {
                "Accept-Language": PChaseModule.configs.loginUserCultureCode,
                "App-ModuleBaseUrl": PChaseModule._PChaseCommon._GetCurrentProjectBaseUrl,
                "Access-Control-Allow-Origin": PChaseModule.configs.basePath,
                "App-CompanyCode": PChaseModule._PChaseCommon._companyCode,
                "App-SiteCode": PChaseModule._PChaseCommon._locationCode,
                "App-CompanyRegion": PChaseModule._PChaseCommon._companyRegion,
                "Access-Control-Allow-Credentials": true,
                "Access-Control-Allow-Methods": 'GET,PUT,POST,DELETE',
                "Access-Control-Allow-Headers": "Origin, X-Requested-With, Content-Type, Accept"
            },
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            success: function (response) {
                if (typeof successCallback === "function") {
                    successCallback(response);
                }
            },
            error: function (response) {
                PChaseModule._PChanseToaster._error(response);
            },
            failure: function (response) {
                PChaseModule._PChanseToaster._failure(response);
            },
            complete: function () {
                LoaderCounter.end();
            }
        });
    }

   
    _PChaseAjaxResponse.prototype.PostAjaxHtmlResponse = function (url, data, successCallback, async = true, loader = true) {
        if (loader) LoaderCounter.start();
        $.ajax({
            url: url,
            data: data,
            type: "POST",
            contentType: false,
            processData: false,
            async: async,
            beforeSend: function () { },
            cache: false,
            headers: {
                "Accept-Language": PChaseModule.configs.loginUserCultureCode,
                "App-ModuleBaseUrl": PChaseModule._PChaseCommon._GetCurrentProjectBaseUrl,
                "Access-Control-Allow-Origin": PChaseModule.configs.basePath,
                "App-CompanyCode": PChaseModule._PChaseCommon._companyCode,
                "App-SiteCode": PChaseModule._PChaseCommon._locationCode,
                "App-CompanyRegion": PChaseModule._PChaseCommon._companyRegion,
                "App-ERPName": PChaseModule._PChaseCommon._erpName,
                "App-EnableOCR": PChaseModule._PChaseCommon._enableOCR,
                "App-NumberOfDecimal": PChaseModule._PChaseCommon._companyNumberOfDecimal,
                "Access-Control-Allow-Credentials": true,
                "Access-Control-Allow-Methods": 'GET,PUT,POST,DELETE',
                "Access-Control-Allow-Headers": "Origin, X-Requested-With, Content-Type, Accept"
            },
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            success: function (response) {
                if (typeof successCallback === "function") {
                    successCallback(response);
                }
            },
            error: function (response) {
                PChaseModule._PChanseToaster._error(response);
            },
            failure: function (response) {
                PChaseModule._PChanseToaster._failure(response);
            },
            complete: function () {

                LoaderCounter.end();

            }
        });
    }

    module._PChaseAjaxResponse = new _PChaseAjaxResponse();
    module._PChaseAjaxResponse.init();
})(PChaseModule || window.PChaseModule);