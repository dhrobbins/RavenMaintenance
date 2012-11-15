var newData = [{Name : "jeff bezos", Company : "Amazon"}, {Name : "Steve Jobs", Company : "Apple"}];
var newStepData = [{State : "Start", Id : "state34", WorkflowName : "Demo"},
					{State : "Middle", Id : "state45", WorkflowName : "Demo"}, 
					{State : "End", Id : "state67", WorkflowName : "Demo"}
				];
var newWorkflowData = [{Id : "wf2222", WorkflowType : "More cowbell"},
						{Id : "wf6767", WorkflowType : "Do It"}
				];

var aoColumnDef = function(className, aoColumnDefs){
	this.className = ko.observable(className);
	this.aoColumnDefs = aoColumnDefs;
};

var ViewModel = function () {
    var self = this;

    self.scriptNames = ko.observableArray([]);

    self.successMessage = ko.observable();
    self.scriptName = ko.observable();
    self.rcName = ko.observable();
    self.scriptSnippet = ko.observable();
	self.scriptResultsTable = {};
	self.scriptResultAODefs = ko.observableArray();
	self.currentAODefColumn = [];
	self.currentClassName = ko.observable();

    self.executeScriptFile = function () {
       $.ajax({
            url: "ExecuteScript/" + self.scriptName(),
            async: true,
            type: "POST",
            data: { },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                var debugWindow = window.open("", "debugWindow", "menubar=1,resizable=1,scrollbars=1");
                debugWindow.document.write(XMLHttpRequest.responseText);
            },
            success: function (msg) {
                var defs = self.getAODefs();
				var data = msg;
				
				self.scriptResultsTable.fnDestroy(true);
				$("#tableArea").append("<table id='scriptResultsTable' class='table table-striped table-bordered dataTable'></table>");
				
				self.scriptResultsTable = $("#scriptResultsTable").dataTable({
					"aoColumnDefs" : defs,
					"aaData" : data
				});
            }
        });
    };

    self.evaluateSnippet = function () {

    }
	
	self.getAODefs = function(){
		var matched = ko.utils.arrayFirst(self.scriptResultAODefs(), function(item){
			return item.className() == self.currentClassName().className();
		});
		
		return matched.aoColumnDefs;
	};
	
	self.getNewData = function(){
		if(self.currentClassName().className() == "Steps") 
			return newStepData;
			
		return newWorkflowData;
	};

    self.getFileScriptNames = function () {
        $.ajax({
            url: "GetFileScriptNames",
            async: true,
            type: "POST",
            data: {},
            /*contentType: "application/json; charset=utf-8",
            dataType: "json",*/
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(XMLHttpRequest.status);
                alert(XMLHttpRequest.responseText);

            },
            success: function (msg) {
                self.scriptNames(ko.utils.arrayMap(msg, function (item) {
                    return item;
                }));
            }
        });
    };
	
	self.initScriptResultsTable = function(){
		self.scriptResultsTable = $("#scriptResultsTable").dataTable();
	};
	
	self.initDataColumnLookup = function(){
		self.scriptResultAODefs.push(new aoColumnDef("Steps", [
				{"mData" : "State", "aTargets" : [0], "sTitle" : "Step (State)"},
				{"mData" : "Id", "aTargets" : [1], "sTitle" : "Step Id"},
				{"mData" : "WorkflowName", "aTargets" : [2], "sTitle" : "Workflow Name"}]));
		
		self.scriptResultAODefs.push(new aoColumnDef("Workflows", [
				{"mData" : "Id", "aTargets" : [0], "sTitle" : "Workflow Id"},
				{"mData" : "WorkflowType", "aTargets" : [1], "sTitle" : "Workflow Type"}]));
				
		self.scriptResultAODefs.push(new aoColumnDef("WorkflowInstances", [
				{"mData" : "Id", "aTargets" : [0], "sTitle" : "Workflow Id"},
				{"mData" : "WorkflowType", "aTargets" : [1], "sTitle" : "Workflow Type"},
				{"mData" : "WorkflowName", "aTargets" : [2], "sTitle" : "Workflow Name"},
				{"mData" : "InitiatedBy", "aTargets" : [3], "sTitle" : "InitiatedBy"}
				]));
	}
	
	self.formatDate = function (theDate) {
        var dt = new Date(theDate);

        return (dt.getMonth() + 1) + "-" + dt.getDate() + "-" + dt.getFullYear() +
                        " " + dt.getHours() + ":" + dt.getMinutes();
    };
}

var vm = new ViewModel();
ko.applyBindings(vm);
vm.getFileScriptNames();
vm.initScriptResultsTable();
vm.initDataColumnLookup();

