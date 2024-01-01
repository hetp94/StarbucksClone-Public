$(document).ready(function () {

    $('#menuList').change(function () {
      
      
        getCategoryList();
    });


    $('#categoryList').change(function () {
        getSubCategoryList();
    });

    $('#subCategoryList').change(function () {
        getProductList();
    });



    $("#sortable").sortable({
        update: function (event, ui) {
            var itemIds = "";
            $("#sortable").find(".taskSingleInline").each(function () {

                var itemId = $(this).attr("data-taskid");
                //alert("Item ID " + itemId);
                itemIds = itemIds + itemId + ",";
            });
            //alert("Item IDs" + itemIds);
            
            document.getElementById("MenuLayout").value = "";
            document.getElementById("MenuLayout").value = itemIds;
            console.log(document.getElementById("MenuLayout").value);
        }
    });

    $("#categorySortable").sortable({
       
        update: function (event, ui) {
            var itemIds = "";
            $("#categorySortable").find(".taskSingleInline").each(function () {

                var itemId = $(this).attr("data-taskid");
                itemIds = itemIds + itemId + ",";
            });
            document.getElementById("CategoryLayout").value = "";
            document.getElementById("CategoryLayout").value = itemIds;
        }
    });

    $("#subCategorySortable").sortable({

        update: function (event, ui) {
            var itemIds = "";
            $("#subCategorySortable").find(".taskSingleInline").each(function () {

                var itemId = $(this).attr("data-taskid");
                itemIds = itemIds + itemId + ",";
            });
            document.getElementById("SubCategoryLayout").value = "";
            document.getElementById("SubCategoryLayout").value = itemIds;
        }
    });


    $("#productSortable").sortable({

        update: function (event, ui) {
            var itemIds = "";
            $("#productSortable").find(".taskSingleInline").each(function () {

                var itemId = $(this).attr("data-taskid");
                itemIds = itemIds + itemId + ",";
            });
            document.getElementById("ProductLayout").value = "";
            document.getElementById("ProductLayout").value = itemIds;
        }
    });
})


//Menu

async function UpdateMenuItem() {

    try {
        const response = await fetch('/Admin/Layout/UpdateMenuItem?' +
            new URLSearchParams({ itemIds: $('#MenuLayout').val() }).toString(), {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            /*body: JSON.stringify({ menuId: $('#menuList').val() })*/
        });

        const data = await response.json();
        console.log(data);
        if (data) {
            alert("Menu Layout Updated!")
        }
        else {
            alert(data);
        }
    } catch (error) {
        console.error(error);
    }


}



//Category

async function getCategoryList() {

    try {
        const response = await fetch(`/Admin/Layout/GetCategory?menuId=${$('#menuList').val()}`);
        if (response.ok) {
            const data = await response.json();
            console.log(data);
            
            var result = JSON.stringify(data);

            await SetCategoryList(result);
            await SetCategoryItems(result);
        }
        else {
            console.error("Something Wrong");
        }
        
    } catch (error) {
        console.error(error);
    }
}

async function SetCategoryList(result) {
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
}

async function SetCategoryItems(result) {
    options = JSON.parse(result);
    document.getElementById("categorySortable").innerHTML = "";
    for (i = 0; i < options.length; i++) {
        var div = document.createElement("div");
        div.setAttribute('id', 'task' + options[i].text);
        div.setAttribute('data-taskid', options[i].value);
        div.setAttribute('class', 'card taskSingleInline mb-3 p-2');
       // div.innerHTML = "<h5>" + options[i].text + " </h5>  ";
        div.innerHTML = "<h5 class='text-capitalize'>" + options[i].text + "</h5>";

        document.getElementById("categorySortable").appendChild(div);
    }

    var updateBtn = document.createElement("a");
    updateBtn.setAttribute('class', 'btn btn-primary py-1 mt-3 mb-3');
    updateBtn.setAttribute('onclick', 'UpdateCategoryItem()');
    updateBtn.innerHTML = "Update Category";
    document.getElementById("categorySortable").appendChild(updateBtn)
}

async function UpdateCategoryItem() {

    try {
        const response = await fetch('/Admin/Layout/UpdateCategoryItem?' +
            new URLSearchParams({ itemIds: $('#CategoryLayout').val() }).toString(), {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            /*body: JSON.stringify({ menuId: $('#menuList').val() })*/
        });

        const data = await response.json();
        console.log(data);
        if (data) {
            alert("Category Layout Updated!")
        }
        else {
            alert(data);
        }
    } catch (error) {
        console.error(error);
    }


}


//Sub Category

async function getSubCategoryList() {
    try {
        const response = await fetch(`/Admin/Layout/GetSubCategory?categoryId=${$('#categoryList').val()}`);
        const data = await response.json();
        console.log(data);
        var result = JSON.stringify(data);

        await SetSubCategoryList(result);
        await SetSubCategoryItems(result);
        //var options = new Array();
        //options = JSON.parse(result);
        //var select = document.getElementById("subCategoryList");
        //select.innerHTML = "";
        //var option = document.createElement("option");
        //option.value = "";
        //option.innerHTML = "";
        //select.appendChild(option);
        //for (i = 0; i < options.length; i++) {
        //    var option = document.createElement("option");
        //    option.value = options[i].value;
        //    option.innerHTML = options[i].text;
        //    select.appendChild(option);
        //}
    } catch (error) {
        console.error(error);
    }
}

async function SetSubCategoryList(result) {
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
}

async function SetSubCategoryItems(result) {
    options = JSON.parse(result);
    document.getElementById("subCategorySortable").innerHTML = "";
    for (i = 0; i < options.length; i++) {
        var div = document.createElement("div");
        div.setAttribute('id', 'task' + options[i].text);
        div.setAttribute('data-taskid', options[i].value);
        div.setAttribute('class', 'card taskSingleInline mb-3 p-2');
        div.innerHTML = "<h5 class='text-capitalize'>" + options[i].text + " </h5>  ";

        document.getElementById("subCategorySortable").appendChild(div);
    }

    var updateBtn = document.createElement("a");
    updateBtn.setAttribute('class', 'btn btn-primary py-1 mt-3 mb-3');
    updateBtn.setAttribute('onclick', 'UpdateSubCategoryItem()');
    updateBtn.innerHTML = "Update Sub Category";
    document.getElementById("subCategorySortable").appendChild(updateBtn)
}

async function UpdateSubCategoryItem() {

    try {
        const response = await fetch('/Admin/Layout/UpdateSubCategoryItem?' +
            new URLSearchParams({ itemIds: $('#SubCategoryLayout').val() }).toString(), {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            /*body: JSON.stringify({ menuId: $('#menuList').val() })*/
        });

        const data = await response.json();
        console.log(data);
        if (data) {
            alert("Sub Category Layout Updated!")
        }
        else {
            alert(data);
        }
    } catch (error) {
        console.error(error);
    }


}


//Product Functions

async function getProductList() {

    try {
        const response = await fetch('/Admin/Layout/GetProduct?' +
            new URLSearchParams({
                menuId: $('#menuList').val(),
                categoryId: $('#categoryList').val(),
                subcategoryId: $('#subCategoryList').val()
            }).toString(), {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',

            },
            /*body: JSON.stringify({ menuId: $('#menuList').val() })*/
        });
        //const response = await fetch(`/Layout/GetProduct?menuId=${$('#menuList').val()}`);
        if (response.ok) {
            const data = await response.json();
            console.log(data);
            var result = JSON.stringify(data);
            await SetProductList(result);
            await SetProductItems(result);
        }
        else {
            console.error("Something Wrong");
        }

    } catch (error) {
        console.error(error);
    }
}

async function SetProductList(result) {
    var options = new Array();
    options = JSON.parse(result);
    var select = document.getElementById("productList");
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

async function SetProductItems(result) {
    options = JSON.parse(result);
    document.getElementById("productSortable").innerHTML = "";
    for (i = 0; i < options.length; i++) {
        var div = document.createElement("div");
        div.setAttribute('id', 'task' + options[i].text);
        div.setAttribute('data-taskid', options[i].value);
        div.setAttribute('class', 'card taskSingleInline mb-3 p-2');
        div.innerHTML = "<h5 class='text-capitalize'>" + options[i].text + " </h5>  ";

        document.getElementById("productSortable").appendChild(div);
    }

    var updateBtn = document.createElement("a");
    updateBtn.setAttribute('class', 'btn btn-primary py-1 mt-3 mb-3');
    updateBtn.setAttribute('onclick', 'UpdateProductItem()');
    updateBtn.innerHTML = "Update Product";
    document.getElementById("productSortable").appendChild(updateBtn)
}

async function UpdateProductItem() {

    try {
        const response = await fetch('/Admin/Layout/UpdateProductItem?' +
            new URLSearchParams({ itemIds: $('#ProductLayout').val() }).toString(), {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            /*body: JSON.stringify({ menuId: $('#menuList').val() })*/
        });

        const data = await response.json();
        console.log(data);
        if (data) {
            alert("Prduct Layout Updated!")
        }
        else {
            alert(data);
        }
    } catch (error) {
        console.error(error);
    }


}


