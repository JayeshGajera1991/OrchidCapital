(function (module) {
    "use strict";
    const _configs = module.configs,
        _root = '#common-aportal-wrapper';

    // Create root
    let _AFinanceNewLoan = function () {
        this.wizardSteps = document.querySelectorAll(".wizard-step");
        this.topSteps = document.querySelectorAll(".loan-step");
        this.processBoxes = document.querySelectorAll(".process-box");
        this.loanApplicationId = document.getElementById("hdnLoanApplicationId").value;
        this.addGuarantor = 1;
    };

    // Page Load After Call This Function
    _AFinanceNewLoan.prototype.init = function () {
        module._AFinanceNewLoan.load();
    }

    _AFinanceNewLoan.prototype.load = function () {
        if (parseInt($("#hdnLoanStep").val()) === 0) {
            AFinanceNewLoan._AFinanceNewLoan.LoanProcessStepBack(1, 0);
        } else {
            let _step = parseInt($("#hdnLoanStep").val());
            AFinanceNewLoan._AFinanceNewLoan.LoanProcessStepBack(_step, (_step - 1));
        }
        $(document).ready(function () {
            $('.select2').select2({
                placeholder: "-- Select --",
                allowClear: true,
                width: '100%'
            });
        });
    }

    _AFinanceNewLoan.prototype.Create = function () {
        document.getElementById('nav_LoanApplication_form').reset();
        document.querySelectorAll('#nav_LoanApplication_form input, #nav_LoanApplication_form textarea').forEach(el => {
            el.value = '';
        });
    }

    _AFinanceNewLoan.prototype.SubmitFormData = function (event, form) {
        $("#nav_LoanApplication_form").removeData("validator");
        $("#nav_LoanApplication_form").removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse($("#nav_LoanApplication_form"));
        event.preventDefault();
        if ($("#nav_LoanApplication_form").valid()) {
            APortalModule._APortalAjaxResponse.PostValidationAjaxJsonResponse(form, function (response) {
                if (response.isSuccess) {
                    APortalModule._APortalToaster._toastr(1, response.message);
                    $("#hdnLoanApplicationId").val(response.id);
                    AFinanceNewLoan._AFinanceNewLoan.loanApplicationId = response.id;
                    AFinanceNewLoan._AFinanceNewLoan.LoanProcessStepBack(2, 1);
                }
                else {
                    APortalModule._APortalToaster._toastr(0, response.message);
                }
            });
        }
    }

    _AFinanceNewLoan.prototype.SubmitLoanApplicationDocumentFormData = function (event, form) {
        $("#nav_LoanApplicationDocument_form").removeData("validator");
        $("#nav_LoanApplicationDocument_form").removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse($("#nav_LoanApplicationDocument_form"));
        event.preventDefault();
        if ($("#nav_LoanApplicationDocument_form").valid()) {
            $("#LoanApplicationId").val(AFinanceNewLoan._AFinanceNewLoan.loanApplicationId);
            APortalModule._APortalAjaxResponse.PostValidationAjaxJsonResponsewithFile(form, function (response) {
                if (response.isSuccess) {
                    APortalModule._APortalToaster._toastr(1, response.message);
                    AFinanceNewLoan._AFinanceNewLoan.LoanProcessStepBack(3, 2);
                }
                else {
                    APortalModule._APortalToaster._toastr(0, response.message);
                }
            });
        }
    }

    _AFinanceNewLoan.prototype.SubmitLoanApplicationGuarantorFormData = function (event, form) {
        $("#nav_LoanApplicationGuarantor_form").removeData("validator");
        $("#nav_LoanApplicationGuarantor_form").removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse($("#nav_LoanApplicationGuarantor_form"));
        event.preventDefault();
        if ($("#nav_LoanApplicationGuarantor_form").valid()) {
            $("#LoanApplicationId").val(AFinanceNewLoan._AFinanceNewLoan.loanApplicationId);
            APortalModule._APortalAjaxResponse.PostValidationAjaxJsonResponsewithFile(form, function (response) {
                if (response.isSuccess) {
                    APortalModule._APortalToaster._toastr(1, response.message);
                    AFinanceNewLoan._AFinanceNewLoan.LoanProcessStepBack(4, 3);
                }
                else {
                    APortalModule._APortalToaster._toastr(0, response.message);
                }
            });
        }
    }

    _AFinanceNewLoan.prototype.LoanProcessStep = function (index) {
        this.wizardSteps.forEach((s, i) => {
            s.style.display = i === index ? "block" : "none";
        });

        // style for top steps and process boxes

        const icons = [
            "fa-solid fa-file-signature",
            "fa-solid fa-folder-open",
            "fa-solid fa-user-group",
            "fa-solid fa-paper-plane"
        ];

        this.topSteps.forEach((step, i) => {
            step.classList.remove("active", "completed");
            const icon = step.querySelector("i");
            icon.className = icons[i];
            if (i < index) {
                step.classList.add("completed");
                icon.className = "fa-solid fa-circle-check";
            }
            else if (i === index) {
                step.classList.add("active");
            }
        });
        // over

        this.processBoxes.forEach((s, i) => {
            s.classList.toggle("active", i === index);
        });
    }

    _AFinanceNewLoan.prototype.LoanProcessStepBack = function (id, index) {
        let url = module.urls.get;
        let data = { Id: index, loanApplicationId: AFinanceNewLoan._AFinanceNewLoan.loanApplicationId };
        APortalModule._APortalAjaxResponse.GetAjaxHtmlResponse(url, data, function (response) {
            $(`${_root} #step${id}`).empty().html(response);
            $(`${_root} #step${id}`).find("#LoanApplicationId").val(AFinanceNewLoan._AFinanceNewLoan.loanApplicationId);
            AFinanceNewLoan._AFinanceNewLoan.LoanProcessStep(index);
        });
    }

    _AFinanceNewLoan.prototype.AddGuarantor = function () {
        var guarantorIndex = parseInt(AFinanceNewLoan._AFinanceNewLoan.addGuarantor);

        const documents = [
            "PAN Card",
            "Aadhaar Card"
        ];

        let documentRows = "";

        for (let j = 0; j < documents.length; j++) {
            const uid = Date.now().toString().slice(-7) + Math.floor(100 + Math.random() * 900);
            documentRows += `
            <tr>
                <td>${documents[j]}</td>

                <td>
                    <input type="hidden"
                        name="Guarantors[${guarantorIndex}].Documents[${j}].DocumentName"
                        value="${documents[j]}" />

                    <div class="file-upload">

                        <label for="GuaImageFile_${uid}" class="upload-button">
                            <i class="fa-solid fa-cloud-arrow-up"></i>
                            <span id="GuauploadText_${uid}">
                                Upload Document
                            </span>
                        </label>

                        <input type="file"
                            id="GuaImageFile_${uid}"
                            name="Guarantors[${guarantorIndex}].Documents[${j}].ImageFile"
                            class="file-input"
                            onchange="AFinanceNewLoan._AFinanceNewLoan.GuarantorFileChange(this, ${uid})" />

                        <span id="GuafileName_${uid}" class="file-name">
                            No file selected
                        </span>

                    </div>
                </td>

                <td>
                    <img src="/loanguarantors/no-image.png"
                        id="GuaimgShow_${uid}"
                        class="document-preview"
                        alt="No Image" />
                        <script>
                            $(document).ready(function () {
                                setGuarantorImageFromPath('/loanguarantors/Guarantors[${guarantorIndex}].Documents[${j}].FilePath',"GuaImageFile_${uid}");
                            });
                        </script>
                </td>
            </tr>`;
        }

        let html = `
            <div class="guarantor-card mb-3">

                <div class="card-title1">
            <h4>Guarantor ${guarantorIndex + 1}</h4>
            </div>

             <div class="form-grid">
                <div class="form-group">
                    <label>Name</label>
                    <input name="Guarantors[${guarantorIndex}].Name" placeholder = "Name" class="form-control txtspace" MaxLength = "500"/>
                </div>

                <div class="form-group">
                    <label>Mobile</label>
                    <input name="Guarantors[${guarantorIndex}].Mobile" placeholder = "Mobile" class="form-control txtspace txtnumeric" MaxLength = "10"/>
                </div>

                <div class="form-group">
                    <label>Occupation</label>
                        <input name="Guarantors[${guarantorIndex}].Occupation" placeholder = "Occupation" class="form-control txtspace" MaxLength = "50"/>
                </div>

                <div class="form-group">
                    <label>Relationship</label>
                    <input name="Guarantors[${guarantorIndex}].Relationship" placeholder = "Relationship" class="form-control txtspace" MaxLength = "50"/>
                </div>

                <div class="form-group">
                    <label>PAN</label>
                    <input name="Guarantors[${guarantorIndex}].PANNumber" placeholder = "PAN Number" class="form-control txtspace" MaxLength = "10"/>
                </div>

                <div class="form-group">
                    <label>Aadhaar</label>
                    <input name="Guarantors[${guarantorIndex}].AadhaarNumber" placeholder = "Aadhaar Number" class="form-control txtnumeric txtspace" MaxLength = "12"/>
                </div>

                <div class="form-group full-width">
                    <label>Address</label>        
                    <textarea name="Guarantors[${guarantorIndex}].Address" class="txtspace" placeholder = "Address" MaxLength = "1000"></textarea>
                </div>
            </div>

                <div class="card-title1">
                    <h4>Guarantor Document</h4>
                </div>

                <table class="table">
                    ${documentRows}
                </table>

                <button type="button"
                        class="removeGuarantor btn btn-red"
                        onclick="AFinanceNewLoan._AFinanceNewLoan.RemoveGuarantor(this);">
                    <i class="fa-solid fa-remove"></i>
                </button>

            </div>`;

        $("#guarantorContainer").append(html);
        AFinanceNewLoan._AFinanceNewLoan.addGuarantor++;
    }

    _AFinanceNewLoan.prototype.RemoveGuarantor = function ($this) {
        $($this).closest(".guarantor-card").remove();
    }

    _AFinanceNewLoan.prototype.FileChange = function (input, index) {
        if (input.files.length > 0) {

            var file = input.files[0];

            $("#fileName_" + index).text(file.name);
            $("#uploadText_" + index).text("Replace Document");

            var url = URL.createObjectURL(file);

            if ($("#image_" + index).length == 0) {
                $(".file-upload").eq(index).append(
                    '<a id="image_' + index + '" class="view-file" target="_blank"><i class="fa fa-eye"></i> View</a>'
                );
            }

            $("#image_" + index).attr("href", url);
            var reader = new FileReader();

            reader.onload = function (e) {
                $("#imgShow_" + index).attr("src", e.target.result);
            };

            reader.readAsDataURL(input.files[0]);
        }
    }

    _AFinanceNewLoan.prototype.GuarantorFileChange = function (input, index) {
        if (input.files.length > 0) {

            var file = input.files[0];

            $("#GuafileName_" + index).text(file.name);
            $("#GuauploadText_" + index).text("Replace Document");

            var url = URL.createObjectURL(file);

            if ($("#Guaimage_" + index).length == 0) {
                $(".file-upload").eq(index).append(
                    '<a id="Guaimage_' + index + '" class="view-file" target="_blank"><i class="fa fa-eye"></i> View</a>'
                );
            }

            $("#Guaimage_" + index).attr("href", url);
            var reader = new FileReader();

            reader.onload = function (e) {
                $("#GuaimgShow_" + index).attr("src", e.target.result);
            };

            reader.readAsDataURL(input.files[0]);
        }
    }

    _AFinanceNewLoan.prototype.UpdateFinalLoanApplicationDetails = function () {
        if ($("#declaration").is(':checked')) {
            let url = module.urls.status;
            let data = { LoanApplicationId: AFinanceNewLoan._AFinanceNewLoan.loanApplicationId };
            APortalModule._APortalAjaxResponse.GetAjaxJsonResponse(url, data, function (response) {
                if (response.isSuccess) {
                    APortalModule._APortalToaster._toastr(1, response.message);
                    window.location.href = "/Finance/LoanApplicationList";
                } else {
                    APortalModule._APortalToaster._toastr(0, response.message);
                }
            });
        } else {
            APortalModule._APortalToaster._toastr(0, 'I hereby declare that all information and uploaded documents are true and correct.');
            return false;
        }
    };


    module._AFinanceNewLoan = new _AFinanceNewLoan();
    module._AFinanceNewLoan.init();
})(AFinanceNewLoan || window.AFinanceNewLoan);