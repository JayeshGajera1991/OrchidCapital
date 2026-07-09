(function (module) {
    "use strict";
    const _configs = module.configs,
        _root = '#common-aportal-wrapper';

    // Create root
    let _APortalCommon = function () {

    };

    // Page Load After Call This Function
    _APortalCommon.prototype.init = function () {
        module._APortalCommon.load();
    }

    _APortalCommon.prototype.load = function () {

    }

    //Check for value ex. return ''
    _APortalCommon.prototype.isNullOrEmptyV = function (value) {
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
    _APortalCommon.prototype.isNullOrEmpty = function (value) {
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

    _APortalCommon.prototype.isNullOrEmptyOrZero = function (value) {
        return (value == null || value == undefined || value == "" || value == "0");
    }

    _APortalCommon.prototype.CloseToggle = function (id, fid) {
        $(`${_root} #${id}`).toggleClass('show');
        document.getElementById(fid).reset();
        document.querySelectorAll(`#${fid} input, #${fid} textarea, #${fid} select`).forEach(el => {
            el.value = '';
        });
    }

    _APortalCommon.prototype.EncryptParameter = function (text) {
        var key = CryptoJS.enc.Utf8.parse("12345678901234567890123456789012"); // 32 chars
        var iv = CryptoJS.enc.Utf8.parse("1234567890123456"); // 16 chars
        var encrypted = CryptoJS.AES.encrypt(
            CryptoJS.enc.Utf8.parse(text),
            key,
            {
                iv: iv,
                mode: CryptoJS.mode.CBC,
                padding: CryptoJS.pad.Pkcs7
            });

        return encrypted.toString(); // Base64
    }

    module._APortalCommon = new _APortalCommon();
    module._APortalCommon.init();
})(APortalModule || window.APortalModule);


function resizeTableHtml() {
    $(".table td:not(.action-wrapper)").each(function () {
        // skip empty or action-heavy cells
        if ($(this).find("button, a, i, input").length > 0) {
            return;
        }
        let text = $(this).clone().children().remove().end().text().trim();
        $(this).addClass('td-ellipsis').attr("data-title", text);
    });

    $(".table-wrapper").each(function () {
        const $wrapper = $(this);
        const wrapperTop = $wrapper.offset().top;
        const bottomSpace = 90; // pagination / footer space

        const availableHeight = $(window).height() - wrapperTop - bottomSpace - 20;

        $wrapper.css({
            "max-height": Math.max(200, availableHeight) + "px",
            "overflow-y": "auto",
            "overflow-x": "auto"
        });
    });
}

$(document).on("mouseenter", ".td-ellipsis", function (e) {
    var text = $(this).attr("data-title");

    $("#customTooltip")
        .text(text)
        .css({
            top: e.pageY + 15,
            left: e.pageX + 15
        })
        .fadeIn(150);
});

$(document).on("mousemove", ".td-ellipsis", function (e) {
    $("#customTooltip").css({
        top: e.pageY + 15,
        left: e.pageX + 15
    });
});

$(document).on("mouseleave", ".td-ellipsis", function () {
    $("#customTooltip").hide();
});
//$(window).on("load resize", resizeLayout);

$(document).ready(function () {
    $('.select2').select2({
        placeholder: "-- Select --",
        allowClear: true,
        width: '100%'
    });
});

$(document).on('keypress', '.txtspace', function (e) {
    if (e.which === 32 && e.target.selectionStart === 0)
        return false;
});

$(".textboxnotallowpaste").bind("paste", function (e) {
    e.preventDefault();
});

$(document).on('keypress', '.txtnumeric', function (e) {
    if (e.which < 48 || e.which > 57) {
        return false;
    }
});

$(document).on('keypress', '.txtdecimal', function (e) {
    var charCode = e.which;
    var value = $(this).val();

    // Allow Backspace
    if (charCode == 8 || charCode == 0) {
        return true;
    }

    // Allow only one decimal point
    if (charCode == 46) {
        if (value.indexOf('.') !== -1) {
            return false;
        }
        return true;
    }

    // Allow only numbers
    if (charCode >= 48 && charCode <= 57) {

        // Check digits after decimal
        if (value.indexOf('.') !== -1) {
            var decimalPart = value.split('.')[1];

            if (decimalPart.length >= 2) {
                return false;
            }
        }

        return true;
    }

    return false;
});

$(document).on('keypress', '.txtchars', function (e) {
    var charCode = e.which;

    // Allow Backspace, Tab, Enter, Space
    if (charCode == 8 || charCode == 9 || charCode == 13 || charCode == 32) {
        return true;
    }

    // Allow only letters
    if ((charCode >= 65 && charCode <= 90) || (charCode >= 97 && charCode <= 122)) {
        return true;
    }

    return false;
});

$(document).on('keypress', '.txtemail', function (e) {
    var charCode = e.which;

    // allow letters, numbers, @ . _ -
    if (
        (charCode >= 48 && charCode <= 57) ||   // numbers
        (charCode >= 65 && charCode <= 90) ||   // uppercase
        (charCode >= 97 && charCode <= 122) ||  // lowercase
        charCode == 64 || // @
        charCode == 46 || // .
        charCode == 95 || // _
        charCode == 45    // -
    ) {
        return true;
    }

    return false;
});

// document.querySelectorAll('#Adminmenu a').forEach(link => {
//     link.addEventListener('click', function () {
//         localStorage.setItem("activeMenu", this.href);
//         $('#Adminmenu li a').removeClass("active");
//     });
// });
// document.querySelectorAll('#profileMenu a').forEach(link => {
//     link.addEventListener('click', function () {
//         localStorage.setItem("activeMenu", this.href);
//         $('#Adminmenu li a').removeClass("active");
//     });
// });
// const saved = localStorage.getItem("activeMenu");
// if (saved === null) {
//     $('#Adminmenu li a:first').addClass("active");
// }
// document.querySelectorAll('#Adminmenu a').forEach(link => {
//     if (link.href === saved) {
//         link.classList.add("active");
//     }
// });

// const profileToggle = document.getElementById("profileToggle");
// const profileMenu = document.getElementById("profileMenu");

// profileToggle.onclick = (e) => {
//     e.stopPropagation();
//     profileMenu.style.display =
//         profileMenu.style.display === "block" ? "none" : "block";
// };

// // close when clicking outside
// document.addEventListener("click", (e) => {
//     if (!profileToggle.contains(e.target) && !profileMenu.contains(e.target)) {
//         profileMenu.style.display = "none";
//     }
// });

// profileToggle.onclick = (e) => {
//     e.stopPropagation();
//     profileMenu.style.display =
//         profileMenu.style.display === "block" ? "none" : "block";

//     profileToggle.classList.toggle("open");
// };