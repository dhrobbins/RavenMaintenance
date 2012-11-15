var MetaDoc = function (tag, id, lastModified) {
    this.tag = ko.observable(tag);
    this.id = ko.observable(id);
    this.lastModified = ko.observable(lastModified);
}

var ViewModel = function () {
    var self = this;

    self.currentMetaDoc = ko.observable();
    self.metaDocDataTable = {};
    self.metaDocs = {};

    self.showMetaDocJson = function () {
        $("#metaDocModal").modal();
    };

    self.closeMetaDocModal = function () {
        $("#metaDocModal").modal("hide");
    };

    self.getMetaDocs = function () {
        $.ajax({
            url: "AllDocsByType",
            async: true,
            type: "POST",
            data: {},
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(XMLHttpRequest.status);
                alert(XMLHttpRequest.responseText);

            },
            success: function (msg) {
                self.metaDocs = ko.utils.parseJson(msg);
                self.metaDocDataTable.fnClearTable();
                self.metaDocDataTable.fnAddData(self.metaDocs);
                self.metaDocDataTable.fnDraw();
            }
        });
    };

    self.initMetaDocDataTable = function () {
        self.metaDocDataTable = $("#metaDocDataTable").dataTable({
            "bProcessing": true,
            "bSort": true,
            "aaSorting": [[0, 'desc']],
            "sPaginationType": "bootstrap",
            "aoColumnDefs": [
                    { "mDataProp": "DocumentType", "aTargets": [0], "bSortable": true, "bSearchable": true },
                    { "mDataProp": "Id", "aTargets": [1], "bSortable": true, "bSearchable": true },
                    {
                        "fnRender": function (oObj) {
                            return self.formatDate(oObj.aData.Created);
                        },
                        "mDataProp": "LastModified",
                        "aTargets": [2],
                        "bSortable": true,
                        "bSearchable": true
                    }
                ],
            "aaData": self.metaDocs,
            "oLanguage": {
                "sSearch": "Search all columns:"
            }
        });
    };

    self.formatDate = function (theDate) {
        var dt = new Date(theDate);

        return (dt.getMonth() + 1) + "-" + dt.getDate() + "-" + dt.getFullYear() +
                        " " + dt.getHours() + ":" + dt.getMinutes();
    };
};

var vm = new ViewModel();
ko.applyBindings(vm);
vm.getMetaDocs();
vm.initMetaDocDataTable();


