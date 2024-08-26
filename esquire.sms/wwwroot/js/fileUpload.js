Dropzone.autoDiscover = false;
$(function () {
    var dz = null;
    $("#fileUpload").dropzone({
        autoProcessQueue: false,
        paramName: "File Upload",
        maxFilesize: 100, //mb
        maxThumbnailFilesize: 1, //mb
        maxFiles: 1,
        parallelUploads: 1,
        acceptedFiles: ".xls,.txt,.xlsx,.csv",
        uploadMultiple: false,

        init: function () {
            dz = this;
            var uploadButton = document.getElementById('btnupload');
            uploadButton.disabled = true;
            $("#btnupload").click(function () {
                $(this).attr("disabled", "disabled");
                $(this).html("Please wait...")
                $(".dz-hidden-input").prop("disabled", true);
                dz.processQueue();
                //$(this).attr("disabled", "disabled");
            });

            dz.on("addedfile", function () {
                if (this.files[1] != null) {
                    this.removeFile(this.files[0]);
                }
                uploadButton.disabled = false;
            });

        },
        success: function (file, response) {
            var preview = $(file.previewElement);
            preview.addClass("dz-success text-success");
            setTimeout(function () {
                dz.removeFile(file);

            }, 2000);
            $("#btnupload").attr("disabled", "disabled");
            $("#btnupload").html("Upload File");
            $(".dz-hidden-input").prop("disabled", false);
            alert(response);

        },
        error: function (file, response) {

            setTimeout(function () {
                dz.removeFile(file);

            }, 2000);
            $("#btnupload").attr("disabled", "disabled");
            $("#btnupload").html("Upload File");
            $(".dz-hidden-input").prop("disabled", false);
            alert(response);
        },


        dictDefaultMessage: "You can drag and drop your file here.",
        dictRemoveFile: "File Remove"
    });

});