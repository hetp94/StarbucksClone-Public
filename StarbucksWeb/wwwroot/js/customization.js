$(document).ready(function () {
    //$('#menuList').change(function () {
    //    $.ajax({
    //        type: "get",
    //        url: "/Product/GetCategory",
    //        data: { menuId: $('#menuList').val() },
    //        datatype: "json",
    //        traditional: true,
    //        success: function (data) {
    //            console.log(data);
    //            debugger;
    //            var result = JSON.stringify(data);
    //            var options = new Array();
    //            options = JSON.parse(result);
    //            var select = document.getElementById("categoryList");
    //            select.innerHTML = "";
    //            var option = document.createElement("option");
    //            option.value = "";
    //            option.innerHTML = "";
    //            select.appendChild(option);
    //            for (i = 0; i < options.length; i++) {
    //                var option = document.createElement("option");
    //                option.value = options[i].value;
    //                option.innerHTML = options[i].text;
    //                select.appendChild(option);
    //            }
    //        }
    //    });
    //});


    //$('#categoryList').change(function () {
    //    $.ajax({
    //        type: "get",
    //        url: "/Product/GetSubCategory",
    //        data: { categoryId: $('#categoryList').val() },
    //        datatype: "json",
    //        traditional: true,
    //        success: function (data) {
    //            console.log(data);
    //            debugger;
    //            var result = JSON.stringify(data);
    //            var options = new Array();
    //            options = JSON.parse(result);
    //            var select = document.getElementById("subCategoryList");
    //            select.innerHTML = "";
    //            var option = document.createElement("option");
    //            option.value = "";
    //            option.innerHTML = "";
    //            select.appendChild(option);
    //            for (i = 0; i < options.length; i++) {
    //                var option = document.createElement("option");
    //                option.value = options[i].value;
    //                option.innerHTML = options[i].text;
    //                select.appendChild(option);
    //            }
    //        }
    //    });
    //});

    $('#CustomizationType').change(function () {
        $.ajax({
            type: "get",
            url: "/Customization/GetSubCustomizationType",
            data: { customizationTypeId: $('#CustomizationType').val() },
            datatype: "json",
            traditional: true,
            success: function (data) {
                console.log(data);
                debugger;
                var result = JSON.stringify(data);
                var options = new Array();
                options = JSON.parse(result);
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
            }
        });
    });

    $('#SubCustomizationType').change(function () {
        $.ajax({
            type: "get",
            url: "/Customization/GetCustomization",
            data: { subCustomizationTypeId: $('#SubCustomizationType').val() },
            datatype: "json",
            traditional: true,
            success: function (data) {
                console.log(data);
                debugger;
                var result = JSON.stringify(data);
                var options = new Array();
                options = JSON.parse(result);
                var select = document.getElementById("Customization");
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
            }
        });
    });

    //$('#Customization').change(function () {
    //    $.ajax({
    //        type: "get",
    //        url: "/Product/GetSubCustomization",
    //        data: { customizationId: $('#Customization').val() },
    //        datatype: "json",
    //        traditional: true,
    //        success: function (data) {
    //            console.log(data);
    //            debugger;
    //            var result = JSON.stringify(data);
    //            var options = new Array();
    //            options = JSON.parse(result);
    //            var select = document.getElementById("SubCustomization");
    //            select.innerHTML = "";
    //            var option = document.createElement("option");
    //            option.value = "";
    //            option.innerHTML = "";
    //            select.appendChild(option);
    //            for (i = 0; i < options.length; i++) {
    //                var option = document.createElement("option");
    //                option.value = options[i].value;
    //                option.innerHTML = options[i].text;
    //                select.appendChild(option);
    //            }
    //        }
    //    });
    //});
});