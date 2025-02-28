/*
* Name         : ggtools.js
* Description  : Main JS application file for GGTools. This file
*                should be included in all layout pages. 
* Last Updated : 2/14/25
* By           : Benjamin Haag
*/

/* ========================================================================================
 * General UI Control
 * ======================================================================================== */

$(document).on("click", "[id*='btnShowHideExtendedFields']", function () {
    var id = $(this).attr("id");
    var status = id.includes("On");
    
    if (status == true) {
        $("#section-extended-search-fields-body").show();
        $("#btnShowHideExtendedFieldsOn").hide();
        $("#btnShowHideExtendedFieldsOff").show();
    }
    else {
        $("#section-extended-search-fields-body").hide();
        $("#btnShowHideExtendedFieldsOff").hide();
        $("#btnShowHideExtendedFieldsOn").show();
    }   
});

function SetControlVisibility() {
    var recordIsAccepted = $("#Entity_IsAcceptedName").val();

    if (recordIsAccepted == "Y") {
        $("#btnSetAcceptedOn").hide();
        $("#btnSetAcceptedOff").show();
        $(".accepted").hide();
    }
    else {
        $("#btnSetAcceptedOn").show();
        $("#btnSetAcceptedOff").hide();
        $(".accepted").show();
    }
}

/* ========================================================================================
 * Modals
  ======================================================================================== */
function OpenLookupModal(type, value_field, display_field) {
    type = type.toLowerCase();
    var modalName = "modal-" + type + "-lookup";
    var overlayName = "search-progress-overlay-" + type;
    var searchResultsSectionName = "section-" + type + "-lookup-search-results";
    var valueHiddenFieldName = modalName + "-value-field";
    var displayHiddenFieldName = modalName + "-display-field";

    //console.log("DEBUG modal is " + modalName + " id field is " + value_field + " name field is " + display_field);

    $("#" + modalName).modal("show");
    $("#" + valueHiddenFieldName).val(value_field);
    $("#" + displayHiddenFieldName).val(display_field);

    $(overlayName).hide();
    // Ensure that the overlays in each modal are hidden by default
    $(".overlay").hide();

    // Clear modal
    $("#section-search-criteria input[type=text]").val("");
    $(searchResultsSectionName).html("");

    // Clear data tables.
    const modal = document.getElementById(modalName);
    clearDataTables(modal);

    $(searchResultsSectionName).find('table.dataTable').each(function () {
        // Clear the DataTable
        $(this).DataTable().clear().draw();
    });

    // Set focus to first visible text control.
    $('.modal').on('shown.bs.modal', function () {
        $(this).find('input:text:visible:first').focus();
    });
}

function clearDataTables(modal) {
    // Find all tables within the modal
    const tables = modal.querySelectorAll('table');

    // Loop through each table and clear its DataTable
    tables.forEach(function (table) {
        if ($.fn.DataTable.isDataTable(table)) {
            // Get the DataTable instance and clear it
            const dataTable = $(table).DataTable();
            dataTable.clear().draw();  // Clear all data and redraw the empty table
            // Clear the search filter
            dataTable.search('').draw(); 
        }
    });
}

function focusFirstVisibleTextBox(modal) {
    // Get all input elements inside the modal
    const inputs = modal.querySelectorAll('input[type="text"]');

    // Loop through inputs and find the first visible one
    for (let i = 0; i < inputs.length; i++) {
        if (inputs[i].offsetParent !== null) { // Check if the input is visible
            inputs[i].focus();  // Set focus to the first visible input
            break;
        }
    }
}

// Toggle show/hide of standard help widget.
$(".fa-info-circle").on('click', function (event) {
    event.stopPropagation();
    event.stopImmediatePropagation();
    /*alert("CLICK");*/
    if ($("#section-panel-help").is(":visible")) {
        //
    } else {
        $("#section-panel-help").show();
    }
});

$(".close").on('click', function (event) {
    $("#section-panel-help").hide();
});

/* ========================================================================================
 * General Utilities
  ======================================================================================== */
function AddRecord() {
    var addNewRecordUrl = $("#hfAddNewRecordLink").val();
    window.location.href = addNewRecordUrl;
}

/* ========================================================================================
 * Datatables
  ======================================================================================== */
function InitDataTableBase(tableName, isBatchEditable) {
    if (isBatchEditable == "Y") {
        InitDataTableWithBatchEdit(tableName);
    }
    else {
        InitDataTable(tableName);
    }
}

function InitDataTable(tableName) {
    tableName = "#" + tableName;

    $(document).ready(function () {
        table = $(tableName).DataTable({
            dom: "Bflrtip",
            stateSave: true,
            responsive: true,
            paging: true,
            "pageLength": 25,
            select: true,
            layout: {
                topStart: {
                    buttons: ['copyHtml5', 'excelHtml5', 'csvHtml5', 'pdfHtml5']
                }
            },
            buttons: [
                {
                    extend: 'copyHtml5',
                    exportOptions: {
                        columns: [0, ':visible']
                    }
                },
                {
                    extend: 'excelHtml5',
                    exportOptions: {
                        columns: ':visible'
                    }
                },
                {
                    extend: 'pdfHtml5',
                    exportOptions: {
                        columns: [0, 1, 2, 5]
                    }
                },
                'colvis',
                'selectAll',
                'selectNone',
                {
                    text: 'Add to Folder',
                    action: function (e, dt, node, config) {
                        OpenAppUserItemFolderModal();
                    }
                },
                {
                    text: 'Edit Selected',
                    action: function (e, dt, node, config) {
                        BatchEdit();
                    }
                },
                {
                    text: 'Edit Filtered',
                    action: function (e, dt, node, config) {
                        BatchEditFiltered();
                    }
                }
            ]
        });

        $('table.ggtools').on('click', 'tr', function () {
            var data = table.row(this).data();
            /*alert('You clicked on ' + data[0] + "'s row");*/
        });
    });
}

function InitDataTableDefault(tableName) {
    tableName = "#" + tableName;

    $(document).ready(function () {
        table = $(tableName).DataTable({
            /*dom: "Bflrtip",*/
            stateSave: true,
            responsive: true,
            paging: true,
            "pageLength": 10,
            select: true,
            buttons: [
                {
                    extend: 'copyHtml5',
                    exportOptions: {
                        columns: [0, ':visible']
                    }
                },
                {
                    extend: 'excelHtml5',
                    exportOptions: {
                        columns: ':visible'
                    }
                },
                {
                    extend: 'pdfHtml5',
                    exportOptions: {
                        columns: [0, 1, 2, 5]
                    }
                },
                'colvis',
                'selectAll',
                'selectNone',
                //{
                //    text: 'Add to Folder',
                //    action: function (e, dt, node, config) {
                //        OpenAppUserItemFolderModal();
                //    }
                //},
                //{
                //    text: 'Edit Selected',
                //    action: function (e, dt, node, config) {
                //        BatchEdit();
                //    }
                //},
                //{
                //    text: 'Edit Filtered',
                //    action: function (e, dt, node, config) {
                //        BatchEditFiltered();
                //    }
                //}
            ]
        });

        $('table.ggtools').on('click', 'tr', function () {
            var data = table.row(this).data();
            /*alert('You clicked on ' + data[0] + "'s row");*/
        });
    });
}



function InitDataTableWithBatchEdit(tableName) {
    tableName = "#" + tableName;

    $(document).ready(function () {
        table = $(tableName).DataTable({
            dom: "Bflrtip",
            responsive: true,
            paging: true,
            "pageLength": 10,
            select: true,
            buttons: [
                {
                    extend: 'copyHtml5',
                    exportOptions: {
                        columns: [0, ':visible']
                    }
                },
                {
                    extend: 'excelHtml5'
                    //exportOptions: {
                    //    columns: [0]
                    //}
                },
                {
                    extend: 'pdfHtml5',
                    exportOptions: {
                        columns: [0, ':visible']
                    }
                },
                'colvis',
                'selectAll',
                'selectNone',
                {
                    text: 'Add to Folder',
                    action: function (e, dt, node, config) {
                        OpenAppUserItemFolderModal();
                    }
                },
                {
                    text: 'Edit Selected',
                    action: function (e, dt, node, config) {
                        BatchEdit();
                    }
                }
            ]
        });



        $('table.ggtools').on('click', 'tr', function () {
            var data = table.row(this).data();
            /*alert('You clicked on ' + data[0] + "'s row");*/
        });
    });
}

function InitDataTableFolderFormat(tableName) {
    tableName = "#" + tableName;

    $(document).ready(function () {
        table = $(tableName).DataTable({
            dom: "Bflrtip",
            stateSave: true,
            responsive: true,
            paging: true,
            "pageLength": 10,
            select: true,
            buttons: [
                {
                    extend: 'copyHtml5',
                    exportOptions: {
                        columns: [0, ':visible']
                    }
                },
                {
                    extend: 'excelHtml5',
                    exportOptions: {
                        columns: ':visible'
                    }
                },
                {
                    extend: 'pdfHtml5',
                    exportOptions: {
                        columns: [0, 1, 2, 5]
                    }
                },
                'colvis',
                'selectAll',
                'selectNone',
                {
                    text: 'Delete Selected Items From Folder',
                    action: function (e, dt, node, config) {
                        BatchDeleteFolderItems();
                    }
                },
                {
                    text: 'Edit Selected',
                    action: function (e, dt, node, config) {
                        BatchEdit();
                    }
                },
                {
                    text: 'Edit Filtered',
                    action: function (e, dt, node, config) {
                        BatchEditFiltered();
                    }
                }
            ]
        });



        $('table.ggtools').on('click', 'tr', function () {
            var data = table.row(this).data();
            /*alert('You clicked on ' + data[0] + "'s row");*/
        });
    });
}

function InitDataTableSingleSelect(tableName) {
    $(document).ready(function () {
        var table = $("#" + tableName).DataTable({
            paging: true,
            "pageLength": 10,
            responsive: true,
            scrollY: '300px',
            select: true,
            searching: true,
            lengthMenu: [[5, 10, 25, 50, -1], [5, 10, 25, 50, "All"]],
            columnDefs: [
                { targets: [0], visible: false }
            ]
        });
    });
}

function InitDataTableLookupFormat(tableName) {
    $(document).ready(function () {
        tableName = "#" + tableName;
        table = $(tableName).DataTable({
            paging: false,
            "bLengthChange": false,
            scrollY: '300px',
            scrollCollapse: true,
            paging: false,
            responsive: true,
            select: {
                style: 'single'
            },
            searching: true,
            columnDefs: [
                { targets: [0], visible: false }
            ]
        });
        table.row(':eq(0)', { page: 'current' }).select();
    });
}

function InitDataTableLookupFormatWithMultiSelect(tableName) {
    $(document).ready(function () {
        tableName = "#" + tableName;
        table = $(tableName).DataTable({
            paging: false,
            stateSave: true,
            "bLengthChange": false,
            scrollY: '300px',
            scrollCollapse: true,
            paging: false,
            responsive: true,
            select: {
                style: 'multi'
            },
            searching: true,
            columnDefs: [
                { targets: [0], visible: false }
            ]
        });
        /*table.row(':eq(0)', { page: 'current' }).select();*/
    });
}

function InitDataTableLight(tableName) {
    $(document).ready(function () {
        tableName = "#" + tableName;
        table = $(tableName).DataTable({
            paging: false,
            stateSave: true,
            "bLengthChange": false,
            scrollY: '350px',
            scrollCollapse: true,
            paging: false,
            responsive: true,
            select: true,
            searching: false,
            columnDefs: [
                { targets: [0], visible: false }
            ]
        });
        /*table.row(':eq(0)', { page: 'current' }).select();*/
    });
}

function InitDataTableWithAssembledName(tableName) {
    $(document).ready(function () {
        tableName = "#" + tableName;
        table = $(tableName).DataTable({
            "bLengthChange": false,
            scrollY: '300px',
            scrollCollapse: true,
            paging: true,
            responsive: true,
            select: {
                style: 'single'
            },
            searching: true,
            columnDefs: [
                {
                    target: 0,
                    visible: false,
                    searchable: false,
                },
                {
                    target: 1,
                    visible: false,
                },
            ]
        });
        table.row(':eq(0)', { page: 'current' }).select();
    });
}

function InitDataTableByClass() {
    $(document).ready(function () {
        table = $("table.ggtools").DataTable({
            dom: 'Blfrtip',
            paging: true,
            "pageLength": 10,
            //initComplete: function () {
            //    SetControlVisibility(tableName);
            //},
            responsive: true,
            buttons: [
                'selectAll',
                'selectNone',
                'csv',
                'excel',
                'pdf',
                {
                    text: 'Batch Edit',
                    action: function (e, dt, node, config) {
                        BatchEdit();
                    }
                }
            ],
            select: true,
            lengthMenu: [[5, 10, 25, 50, -1], [5, 10, 25, 50, "All"]],
            columnDefs: [
                { targets: [0], visible: false }
            ]
        });

        $('table.ggtools').on('click', 'tr', function () {
            var data = table.row(this).data();
            /*alert('You clicked on ' + data[0] + "'s row");*/
        });
    });
}

function InitDataTableLightMultiSelect(tableName) {
    $(document).ready(function () {
        var table = $("#" + tableName).DataTable({
            paging: false,
            responsive: true,
            buttons: [
                'selectAll',
                'selectNone',
            ],
            stateSave: true,
            "bLengthChange": false,
             
            select: {
                style: 'multi'
            },
            searching: true,
            columnDefs: [
                {
                    target: 0,
                    visible: false,
                    searchable: false,
                }
            ]
        });
    });
}

function InitDataTableDualSelectFormat(tableName) {
    $(document).ready(function () {
        var table = $("#" + tableName).DataTable({
            paging: false,
            responsive: true,
            buttons: [
                'selectAll',
                'selectNone',
            ],
            stateSave: true,
            "bLengthChange": false,
            scrollY: '300px',
            select: {
                style: 'multi'
            },
            searching: false,
            columnDefs: [
                {
                    target: 0,
                    visible: false,
                    searchable: false,
                }
            ]
        });
    });
}

function GetSelectedEntityIDs(tableName) {
    var table = $('#' + tableName).DataTable();
    var ids = $.map(table.rows('.selected').data(), function (item) {
        return item[0]
    });
    //console.log(ids)
    return ids;
}

function GetFilteredEntityIDs(tableName) {
    // Assuming you have initialized your DataTable with an id 'example'
    var table = $('#' + tableName).DataTable();

    // Array to store values from the first column of filtered rows
    var firstColumnValues = [];

    // Get values from the first column of filtered rows
    table.column(0, { filter: 'applied' }).data().each(function (value) {
        // Add value to the firstColumnValues array
        firstColumnValues.push(value);
    });

    // Now you have the values from the first column of only the filtered rows in the firstColumnValues array
    //console.log(firstColumnValues);
    return firstColumnValues;
}

function GetSelectedSpeciesIDs(tableName) {
    var table = $('#' + tableName).DataTable();
    var ids = $.map(table.rows('.selected').data(), function (item) {
        return item[2]
    });
    //console.log(ids)
    return ids;
}

function GetSelectedAppUserItemListIDs(tableName) {
    var table = $('#' + tableName).DataTable();
    var ids = $.map(table.rows('.selected').data(), function (item) {
        return item[1]
    });
    return ids;
}

function GetSelectedEntityStringIDs(tableName) {
    var table = $('#' + tableName).DataTable();
    var ids = $.map(table.rows('.selected').data(), function (item) {
        return "'" + item[0] + "'"
    });
    //console.log(ids)
    return ids;
}

function GetSelectedEntityLabels(tableName) {
    var table = $('#' + tableName).DataTable();
    var ids = $.map(table.rows('.selected').data(), function (item) {
        return item[3]
    });
    //console.log(ids)
    return ids;
}

function GetSelectedEntityQuotedStrings(tableName) {
    var table = $('#' + tableName).DataTable();
    var ids = $.map(table.rows('.selected').data(), function (item) {
        return "'" + item[1] + "'";
    });
    //console.log(ids)
    return ids;
}

function GetSelectedEntityText(tableName) {
    var table = $('#' + tableName).DataTable();
    var ids = $.map(table.rows('.selected').data(), function (item) {
        return item[1]
    });
    //console.log(ids)
    return ids;
}


/* ========================================================================================
 * Edit logic
  ======================================================================================== */

/* 
 Name           : SetReadOnly()
 Description    : Iterates through all input controls within a designated div, and clears the
                  contents of each.
 Notes          : Requires that all input fields be within a div named "section-input-fields."
 */
function SetReadOnly() {

    // Disable all input controls.
    $('#section-input-fields').find('input, select, textarea').prop('disabled', true);
    $("#section-file-input-fields input").attr('readonly', true);
    $("#section-file-input-fields input").prop('disabled', true);
    $('section-file-input-fields input[type=checkbox]').attr('disabled', 'true');

    // TODO Needed?
    $("#section-edit-controls").hide();
    $(".edit-controls").hide();

    // Hide all buttons that are explicitly used to make changes.
    $('button').each(function () {
        if ($(this).hasClass('edit-enabled')) {
            $(this).hide();
        }
    });

    $("#btnSaveEdits").hide();
    $("#btnReset").hide();
    $("#btnDelete").hide();
}

/* ========================================================================================
 * Search logic
  ======================================================================================== */
$(document).on("click", "[id='btnSearch']", function () {
    var eventAction = $(this).data("ggtools-event-action");
    var eventValue = $(this).data("ggtools-event-value");
    $("#EventAction").val(eventAction);
    $("#EventValue").val(eventValue);
});

$(document).on("click", "[id='btnReset']", function () {
    Reset();
});

/* If the user resets the search, 1) Remove all search criteria, and 2) Clear the search-results
 * Datatable.*/
function Reset() {
    $(this).closest('form').find("input[type=text], textarea").val("");
    $("#section-search-criteria select").val("");
    $("#section-search-criteria input[type=text]").val("");
    $("textarea").val("");
    $("#ItemIDList").val("");
    $("#EventValue").val("");
    $('input:checkbox').removeAttr('checked');

    // Clear hidden fields used to support date-range searches./
    $("#SearchEntity_CreatedDateText").val("");
    $("#SearchEntity_CreatedDateFrom").val("");
    $("#SearchEntity_CreatedDateTo").val("");

    $("#SearchEntity_ModifiedDateText").val("");
    $("#SearchEntity_ModifiedDateFrom").val("");
    $("#SearchEntity_ModifiedDateTo").val("");

    $("#EventAction").val("RESET");
    $.fn.dataTable.tables({ api: true }).clear().draw();
}

$("#btnModifiedByDateClear").on('click', function (event) {
    $("#SearchEntity_ModifiedDate").val("");
    $("#SearchEntity_ModifiedDateFrom").val("");
    $("#SearchEntity_ModifiedDateTo").val("");

    // NOTE: With the addition of saved-search list on each page, the main data table will be
    // the second one on the page.
    //var table = $('#' + $.fn.dataTable.tables()[1].id).DataTable();

    // Operates on all tables on the page.
    $.fn.dataTable
        .tables({ visible: true, api: true })
        .clear()
        .draw();
});

$(document).on("click", "[id='btnShowHideExtendedFields']", function () {
    var eventValue = $("#EventValue").val();
    if (eventValue == "EXTENDED") {
        /*alert("DEBUG HIDE EXTENDED");*/
        $("#EventValue").val("");
    }
    else {
        /*alert("DEBUG SHOW EXTENDED");*/
        $("#EventValue").val("EXTENDED");
    }
});

function SetExtendedFields() {
    var eventValue = $("#EventValue").val();
    if (eventValue == "EXTENDED") {
        $("#section-extended-search-fields").removeClass("collapsed-card");
        $("#section-extended-search-fields-body").show();
        $("#btnShowHideExtendedFieldsIcon").removeClass("fas fa-plus");
        $("#btnShowHideExtendedFieldsIcon").addClass("fas fa-minus");
        
    }
}

/* ========================================================================================
 * Note Logic
  ======================================================================================== */
function SearchNotes(link) {

    var tableName = $("#TableName").val();
    var note = $("#txtLookupNote").val();

    var formData = new FormData();
    formData.append("TableName", tableName);
    formData.append("Note", note);

    $.ajax({
        url: link,
        type: 'POST',
        cache: false,
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            $("#section-lookup-note-search-results").html(response);
        },
        error: function () {
            alert("Error");
        }
    });
}

function GetSelectedNote(tableName) {
    var table = $('#' + tableName).DataTable();
    var ids = $.map(table.rows('.selected').data(), function (item) {
        return item[1]
    });
    return ids;
}

/* 
========================================================================================
Accepted-Name Control Logic
======================================================================================== 
*/
$(document).on("click", "[id*='btnSetAccepted']", function () {
    var id = $(this).attr("id");
    var status = id.includes("On");
    ToggleAcceptedNameControls(status);
});

function SetControlVisibility() {
    var recordIsAccepted = $("#Entity_IsAcceptedName").val();

    if (recordIsAccepted == "Y") {
        $("#btnSetAcceptedOn").hide();
        $("#btnSetAcceptedOff").show();
        $(".accepted").hide();
    }
    else {
        $("#btnSetAcceptedOn").show();
        $("#btnSetAcceptedOff").hide();
        $(".accepted").show();
    }
}

function ToggleAcceptedNameControls(status) {
    var entityId = $("#Entity_ID").val();

    // If status is set to "ACCEPTED", set accepted (current) ID equal to the species ID.
    // Otherwise, use the specified accepted-name ID.
    if (status == true) {
        $("#Entity_AcceptedID").val(entityId);
        $("#Entity_AcceptedName").val("");

        $("#Entity_IsAcceptedName").val("Y");
        $("#btnSetAcceptedOn").hide();
        $("#btnSetAcceptedOff").show();
        $(".accepted").hide();
    }
    else {
        $("#Entity_IsAcceptedName").val("N");
        $("#btnSetAcceptedOn").show();
        $("#btnSetAcceptedOff").hide();
        $(".accepted").show();
    }

}

function SetAcceptedNameControls(selectorControlId) {
    var entityId = $("#Entity_ID").val();
    $(".accepted").hide();
    if ($('#' + selectorControlId).prop('checked') == true) {
        $("#Entity_IsAcceptedName").val("Y");
        $(".accepted").hide();
        $("#Entity_AcceptedID").val(entityID);
    }
    else {
        $("#Entity_IsAcceptedName").val("N ");
        $(".accepted").show();
    }
}
