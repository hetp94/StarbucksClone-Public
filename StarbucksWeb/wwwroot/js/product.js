$(document).ready(function () {
    $('#menuList').change(function () {
        getCategoryList();
    });


    $('#categoryList').change(function () {
        getSubCategoryList();
    });

    $('#CustomizationType').change(function () {
        getSubCustomizationTypeList();
    });

    $('#SubCustomizationType').change(function () {
        getCustomizationList();
    });

    $('#Customization').change(function () {
        getSubCustomizationList();
    });


    $('#CustomizationCategory').change(function () {
        getCustomizationSubcategoryList();
    });

    $('#CustomizationSubcategory').change(function () {
        getCustomizationDropDown();
    });

    $('#CustomizationId').change(function () {
        getCustomizationOptions();
    });

});


async function getCategoryList() {
    try {
        //const response = await fetch('/Product/GetCategory', {
        //    method: 'GET',
        //    headers: {
        //        'Content-Type': 'application/json'
        //    },
        //    body: JSON.stringify({ menuId: $('#menuList').val() })
        //});
        const response = await fetch(`/Admin/Product/GetCategory?menuId=${$('#menuList').val()}`);
        const data = await response.json();
        console.log(data);
        debugger;
        var result = JSON.stringify(data);
        var options = new Array();
        options = JSON.parse(result);
        var select = document.getElementById("categoryList");
        select.innerHTML = "";
        var option = document.createElement("option");
        option.value = "";
        option.innerHTML = "";
        select.appendChild(option);
        for (i = 0; i < options.length; i++) {
            var option = document.createElement("option");
            option.value = options[i].value;
            option.innerHTML = options[i].text;
            select.appendChild(option);
        }
    } catch (error) {
        console.error(error);
    }
}

async function getSubCategoryList() {
    try {
        const response = await fetch(`/Admin/Product/GetSubCategory?categoryId=${$('#categoryList').val()}`);
        const data = await response.json();
        console.log(data);
        var result = JSON.stringify(data);
        var options = new Array();
        options = JSON.parse(result);
        var select = document.getElementById("subCategoryList");
        select.innerHTML = "";
        var option = document.createElement("option");
        option.value = "";
        option.innerHTML = "";
        select.appendChild(option);
        for (i = 0; i < options.length; i++) {
            var option = document.createElement("option");
            option.value = options[i].value;
            option.innerHTML = options[i].text;
            select.appendChild(option);
        }
    } catch (error) {
        console.error(error);
    }
}

async function getSubCustomizationTypeList() {
    try {
        const response = await $.ajax({
            type: "get",
            url: "/Admin/Product/GetSubCustomizationType",
            data: { customizationTypeId: $('#CustomizationType').val() },
            dataType: "json",
            traditional: true,
        });
        console.log(response);
        var options = JSON.parse(JSON.stringify(response));
        var select = document.getElementById("SubCustomizationType");
        select.innerHTML = "";
        var option = document.createElement("option");
        option.value = "";
        option.innerHTML = "";
        select.appendChild(option);
        for (i = 0; i < options.length; i++) {
            var option = document.createElement("option");
            option.value = options[i].value;
            option.innerHTML = options[i].text;
            select.appendChild(option);
        }
    } catch (error) {
        console.error(error);
    }
}

async function getCustomizationList() {
    try {
        const response = await $.ajax({
            type: "get",
            url: "/Admin/Product/GetCustomization",
            data: { subCustomizationTypeId: $('#SubCustomizationType').val() },
            datatype: "json",
            traditional: true,
        });
        console.log(response);
        debugger;
        const options = JSON.parse(JSON.stringify(response));
        const select = document.getElementById("Customization");
        select.innerHTML = "";
        const option = document.createElement("option");
        option.value = "";
        option.innerHTML = "";
        select.appendChild(option);
        for (let i = 0; i < options.length; i++) {
            const option = document.createElement("option");
            option.value = options[i].value;
            option.innerHTML = options[i].text;
            select.appendChild(option);
        }
    } catch (error) {
        console.log(error);
    }
}

async function getSubCustomizationList() {
    try {
        const response = await $.ajax({
            type: "get",
            url: "/Admin/Product/GetSubCustomization",
            data: { customizationId: $('#Customization').val() },
            datatype: "json",
            traditional: true
        });
        console.log(response);
        debugger;
        const options = JSON.parse(JSON.stringify(response));
        const select = document.getElementById("SubCustomization");
        select.innerHTML = "";
        const option = document.createElement("option");
        option.value = "";
        option.innerHTML = "";
        select.appendChild(option);
        for (let i = 0; i < options.length; i++) {
            const option = document.createElement("option");
            option.value = options[i].value;
            option.innerHTML = options[i].text;
            select.appendChild(option);
        }
    } catch (error) {
        console.error(error);
    }
}

async function getCustomizationSubcategoryList() {
    try {
        const response = await fetch(`/Admin/Product/GetCustomizationSubcategoryList?CustomizationCategoryId=${$('#CustomizationCategory').val()}`);
        const data = await response.json();
        console.log(data);
        var result = JSON.stringify(data);
        var options = new Array();
        options = JSON.parse(result);
        var select = document.getElementById("CustomizationSubcategory");
        select.innerHTML = "";
        var option = document.createElement("option");
        option.value = "";
        option.innerHTML = "";
        select.appendChild(option);
        for (i = 0; i < options.length; i++) {
            var option = document.createElement("option");
            option.value = options[i].value;
            option.innerHTML = options[i].text;
            select.appendChild(option);
        }
    } catch (error) {
        console.error(error);
    }
}


async function getCustomizationDropDown() {
    try {
        const response = await fetch(`/Admin/Product/GetCustomizationDropDown?CustomizationSubCategoryId=${$('#CustomizationSubcategory').val()}`);
        const data = await response.json();
        console.log(data);
        var result = JSON.stringify(data);
        var options = new Array();
        options = JSON.parse(result);
        var select = document.getElementById("CustomizationId");
        select.innerHTML = "";
        var option = document.createElement("option");
        option.value = "";
        option.innerHTML = "";
        select.appendChild(option);
        for (i = 0; i < options.length; i++) {
            var option = document.createElement("option");
            option.value = options[i].value;
            option.innerHTML = options[i].text;
            select.appendChild(option);
        }
    } catch (error) {
        console.error(error);
    }
}

async function getCustomizationOptions() {
    try {
        const response = await fetch(`/Admin/Product/GetCustomizationOptionsList?CustomizationId=${$('#CustomizationId').val()}`);
        const data = await response.json();
        console.log(data);
        var result = JSON.stringify(data);
        var options = new Array();
        options = JSON.parse(result);
        var select = document.getElementById("CustomizationOption");
        select.innerHTML = "";
        var option = document.createElement("option");
        option.value = "";
        option.innerHTML = "";
        select.appendChild(option);
        for (i = 0; i < options.length; i++) {
            var option = document.createElement("option");
            option.value = options[i].value;
            option.innerHTML = options[i].text;
            select.appendChild(option);
        }
    } catch (error) {
        console.error(error);
    }
}

var _row = null;
//function addCustomization() {
//    debugger;
//    var flag = true;
//    $("#btnUpdateCustomization").hide();
//    $("#btnRemoveCustomization").hide();

//    var CustomizationType = document.getElementById("CustomizationType").value;
//    var SubCustomizationType = document.getElementById("SubCustomizationType").value;
//    var Customization = document.getElementById("Customization").value;
//    var SubCustomization = document.getElementById("SubCustomization").value;
//    var Qty = document.getElementById("Qty").value;

//    debugger;
//    if (flag == true) {

//        $("#customizationTable tbody").append("<tr>" +
//            "<td class=\"Custom\">" + "<a style='color:blue' onclick=\"editCustomization(this)\" href='#'>Edit</a>" + "</td>" +
//            "<td class=\"Custom\">" + "<a style='color:red' onclick=\"removeCustomization(this)\" href='#'>Delete</a>" + "</td>" +
//            "<td class=\"Custom\">" + CustomizationType + "</td>" +
//            "<td class=\"Custom\">" + SubCustomizationType + "</td>" +
//            "<td class=\"Custom\">" + Customization + "</td>" +
//            "<td class=\"Custom\">" + SubCustomization + "</td>" +
//            "<td class=\"Custom\">" + Qty + "</td>" +
//            "</tr>");

//        ClearAllFields();
//    }
//}

function addCustomization() {
    debugger;
    var flag = true;

   
    //let selecetedIndex = CustomizationCategoryDropDown.selectedIndex;
    //console.log("Selected index is: " + selecetedIndex);
    //let selectedOption = CustomizationCategoryDropDown.options[selecetedIndex];
    //console.log("Selected option is: " + selectedOption.outerHTML);
    //console.log("Selected value is: " + selectedOption.value);
    //console.log("Selected text is: " + selectedOption.text);

    $("#btnUpdateCustomization").hide();
    $("#btnRemoveCustomization").hide();
    let CustomizationCategoryDropDown = document.getElementById('CustomizationCategory');
    var customizationCategoryText = CustomizationCategoryDropDown.options[CustomizationCategoryDropDown.selectedIndex].text;
    var customizationCategoryValue = CustomizationCategoryDropDown.options[CustomizationCategoryDropDown.selectedIndex].value;


    let CustomizationSubcategoryDropDown = document.getElementById('CustomizationSubcategory');
    var CustomizationSubcategoryText = CustomizationSubcategoryDropDown.options[CustomizationSubcategoryDropDown.selectedIndex].text;
    var CustomizationSubcategoryValue = CustomizationSubcategoryDropDown.options[CustomizationSubcategoryDropDown.selectedIndex].value;

    let CustomizationIdDropDown = document.getElementById('CustomizationId');
    var CustomizationIdText = CustomizationIdDropDown.options[CustomizationIdDropDown.selectedIndex].text;
    var CustomizationIdValue = CustomizationIdDropDown.options[CustomizationIdDropDown.selectedIndex].value;

    let CustomizationOptionDropDown = document.getElementById('CustomizationOption');
    var CustomizationOptionText = CustomizationOptionDropDown.options[CustomizationOptionDropDown.selectedIndex].text;
    var CustomizationOptionValue = CustomizationOptionDropDown.options[CustomizationOptionDropDown.selectedIndex].value;

    var Qty = document.getElementById("Qty").value;




    debugger;
    if (flag == true) {

        $("#customizationTable tbody").append("<tr>" +
            "<td class=\"Custom\">" + "<a style='color:blue' onclick=\"editCustomization(this)\" href='#'>Edit</a>" + "</td>" +
            "<td class=\"Custom\">" + "<a style='color:red' onclick=\"removeCustomization(this)\" href='#'>Delete</a>" + "</td>" +
            "<td class=\"Custom\">" + customizationCategoryText + "</td>" +
            "<td class=\"Custom\">" + CustomizationSubcategoryText + "</td>" +
            "<td class=\"Custom\">" + CustomizationIdText + "</td>" +
            "<td class=\"Custom\">" + CustomizationOptionText + "</td>" +
            "<td class=\"Custom\">" + Qty + "</td>" +
            "<td class=\"visually-hidden\">" + customizationCategoryValue + "</td>" +
            "<td class=\"visually-hidden\">" + CustomizationSubcategoryValue + "</td>" +
            "<td class=\"visually-hidden\">" + CustomizationIdValue + "</td>" +
            "<td class=\"visually-hidden\">" + CustomizationOptionValue + "</td>" +

            "</tr>");

        ClearAllFields();
    }
}

function removeCustomization(row_id) {
    $(row_id).parents("tr").remove();
    _row = $(row_id).parents("tr");

    $("#btnAddCustomization").show();
    $("#btnUpdateCustomization").hide();
    $("#btnRemoveCustomization").hide();

    ClearAllFields();
}

//async function editCustomization(row_id) {
//    $("#btnAddCustomization").hide();
//    $("#btnUpdateCustomization").show();
//    $("#btnRemoveCustomization").show();

//    _row = $(row_id).parents("tr");
//    var cols = _row.children("td");
//    console.log(1);

//    var CustomizationType = $(cols[2]).text();
//    document.getElementById("CustomizationType").value = CustomizationType;
//    debugger;
//    await getSubCustomizationTypeList();
//    console.log(2);

//    var SubCustomizationType = ($(cols[3]).text());
//    document.getElementById("SubCustomizationType").value = SubCustomizationType;
//    await getCustomizationList();
//    console.log(3);

//    var Customization = ($(cols[4]).text());
//    document.getElementById("Customization").value = Customization;
//    await getSubCustomizationList();
//    console.log(4);

//    var SubCustomization = ($(cols[5]).text());
//    document.getElementById("SubCustomization").value = SubCustomization;
//    console.log(5);

//    var Qty = ($(cols[6]).text());
//    document.getElementById("Qty").value = Qty;
//    console.log(6);
//}

async function editCustomization(row_id) {
    $("#btnAddCustomization").hide();
    $("#btnUpdateCustomization").show();
    $("#btnRemoveCustomization").show();

    _row = $(row_id).parents("tr");
    var cols = _row.children("td");
    console.log(1);

    var CustomizationCategory = $(cols[7]).text();
    document.getElementById("CustomizationCategory").value = CustomizationCategory;
    debugger;
    await getCustomizationSubcategoryList();
    console.log(2);

    var CustomizationSubcategory = ($(cols[8]).text());
    document.getElementById("CustomizationSubcategory").value = CustomizationSubcategory;
    await getCustomizationDropDown();
    console.log(3);

    var CustomizationId = ($(cols[9]).text());
    document.getElementById("CustomizationId").value = CustomizationId;
    await getCustomizationOptions();
    console.log(4);

    var CustomizationOption = ($(cols[10]).text());
    document.getElementById("CustomizationOption").value = CustomizationOption;
    console.log(5);

    var Qty = ($(cols[6]).text());
    document.getElementById("Qty").value = Qty;
    console.log(6);
}

function updateCustomization() {
    $("#btnAddCustomization").show();
    $("#btnUpdateCustomization").hide();
    $("#btnRemoveCustomization").hide();

    $(_row).after(addCustomization());
    $(_row).remove();

    ClearAllFields();
}

function cancelCustomization() {
    $("#btnAddCustomization").show();
    $("#btnUpdateCustomization").hide();
    $("#btnRemoveCustomization").hide();

    ClearAllFields();
}


//function ClearAllFields() {
//    document.getElementById("CustomizationType").value = "";
//    document.getElementById("SubCustomizationType").value = "";
//    document.getElementById("Customization").value = "";
//    document.getElementById("SubCustomization").value = "";
//    document.getElementById("Qty").value = "";
//}

function ClearAllFields() {
    document.getElementById("CustomizationCategory").value = "";
    document.getElementById("CustomizationSubcategory").value = "";
    document.getElementById("CustomizationId").value = "";
    document.getElementById("CustomizationOption").value = "";
    document.getElementById("Qty").value = "";
}


function CreateCustomizationObject() {
    if (document.getElementById("customizationTable") != null) {
        debugger;
        document.getElementById("CustomizationArray").value = "";
        var customizationArray = new Array();
        $("table#customizationTable tbody tr").each(function () {
            var row = $(this);
            var customization = {};
            customization.CustomizationCategoryId = row.find("td").eq(7).html();
            customization.CustomizationSubcategoryId = row.find("td").eq(8).html();
            customization.CustomizationId = row.find("td").eq(9).html();
            customization.CustomizationOptionId = row.find("td").eq(10).html();
            customization.Qty = row.find("td").eq(6).html();
            customizationArray.push(customization);
            console.log(customization);
        });
        var result = JSON.stringify(customizationArray);
        result = result.replace(/&amp;/g, '&');
        document.getElementById("CustomizationArray").value = result;
        console.log(result);
        debugger;
    }
}

function SaveProduct() {
    CreateCustomizationObject();
    var GroupData = $("#myform").serialize();
    console.log(GroupData);
    console.log($("#myform").serializeArray());

    var frm = $('#myform');
    var formData = new FormData(frm[0]);
    //formData.append('file', $('input[type=file]')[0].files[0]);

    formData.append('file', $('#Product_WholeImage'));
    formData.append('file', $('#Product_CroppedImage'));
    GroupData = formData;

    $.ajax({
        type: "POST",
        url: "/Admin/Product/SaveProduct",
        data: GroupData,
        processData: false,
        contentType: false,
        //contentType: "multipart/form-data",
        //dataType: "json",
        success: function (res) {
            if (res.success == false) {
                alert(res.error);
            }
            else if (res.success == true) {
                alert("Product Added");
            }
        },
        error: function (res) {
            alert("Error: " + JSON.stringify(res));
        }
    });
}



function UpdateProduct() {
    CreateCustomizationObject();
    var GroupData = $("#myform").serialize();
    console.log(GroupData);
    console.log($("#myform").serializeArray());

    var frm = $('#myform');
    var formData = new FormData(frm[0]);
    //formData.append('file', $('input[type=file]')[0].files[0]);

    formData.append('file', $('#Product_WholeImage'));
    formData.append('file', $('#Product_CroppedImage'));
    GroupData = formData;

    $.ajax({
        type: "POST",
        url: "/Admin/Product/UpdateProductVM",
        data: GroupData,
        processData: false,
        contentType: false,
        //contentType: "multipart/form-data",
        //dataType: "json",
        success: function (res) {
            if (res.success == false) {
                alert(res.error);
            }
            else if (res.success == true) {
                alert("Product Updated!");
            }
        },
        error: function (res) {
            alert("Error: " + JSON.stringify(res));
        }
    });
}