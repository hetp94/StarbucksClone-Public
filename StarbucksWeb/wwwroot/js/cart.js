


$(document).ready(function () {
    var objArray = new Array();

    /* alert(localStorage.getItem("objKey"));*/








    $('#cartBtn').click(function () {
        var dataObj = localStorage.getItem("objKey");
        /*    alert(dataObj);*/
        console.log("dataObj " + dataObj);
        //console.log("objArray "+ objArray.ProductId);
        $.ajax({
            type: "POST",
            url: "/Customer/Menu/CartData",
            data: dataObj,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                window.location.href = "/Customer/Menu/Cart"
            },
            error: function (xhr, status, error) {
                console.log(error);
            }
        });



    });

    $('#checkoutBtn').click(function () {
        var dataObj = localStorage.getItem("objKey");
        if (!dataObj || dataObj == 'null') {
            window.location.href = '/customer/menu';
        }
        else {
            var token = $('input[name="__RequestVerificationToken"]').val();
            //console.log("objArray "+ objArray.ProductId);
            $.ajax({
                type: "POST",
                url: "/Customer/Menu/CheckOut1",
                data: dataObj,
                dataType: "json",
                headers: {
                    // Include the Anti-Forgery Token in the request headers
                    RequestVerificationToken: token
                },
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    if (result) {
                        window.location.href = "/Customer/Menu/CheckOut"
                    }
                    else {
                        alert(result);
                    }
                },
                error: function (xhr, status, error) {
                    console.log(error);
                }
            });
        }
       



    });

    GetProductData();

})


function GetProductData() {
    var dataObj = localStorage.getItem("objKey");
    console.log("dataObj: " + dataObj);
    if (!dataObj || dataObj == 'null') {

        var container = $("#cartDetailsContainer");
        var messagediv = $("<div>").addClass("mt-5 m-3 p-3");
        container.append(messagediv);
        var message = $("<h3>").addClass("text-capitalize").text('Your Cart is Empty');
        messagediv.append(message);
        var buttonElement = $("<a>").addClass("btn btn-success m-3 p-3 py-2").text('Add Items').attr("href", "/customer/menu");
        messagediv.append(buttonElement);
    }
    else {
        $.ajax({
            type: "POST",
            url: "/Customer/Menu/CartData",
            data: dataObj,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                if (result == "") {
                }
                else {
                    renderCart(result.productList);
                }
            },
            error: function (xhr, status, error) {
                console.log(error);
            }
        });
    }

}




//function createProductCard(product) {
//    console.log(product);
//    var card = document.createElement("div");
//    card.className = "card shadow p-3 mb-3 col-xxl-6 col-xl-9 col-lg-9 col-md-9 col-sm-12 mx-auto mt-3 rounded-3";

//    var row = document.createElement("div");
//    row.className = "row mb-3";

//    var imageCol = document.createElement("div");
//    imageCol.className = "col-lg-3 col-md-8 col-sm-12 pb-3";
//    var image = document.createElement("img");
//    image.src = product.imageUrl;
//    image.style.width = "110px";
//    image.style.borderRadius = "50%";
//    imageCol.appendChild(image);

//    var infoCol = document.createElement("div");
//    infoCol.className = "col-lg-7 col-md-9 col-sm-12";
//    var productName = document.createElement("h4");
//    productName.className = "text-capitalize";
//    productName.textContent = product.name;
//    var size = document.createElement("p");
//    size.textContent = "Grande";
//    var rewardPoints = document.createElement("p");
//    rewardPoints.textContent = product.rewardPoints + " ";
//    var starIcon = document.createElement("i");
//    starIcon.className = "fa-solid fa-star fa-2xs";
//    rewardPoints.appendChild(starIcon);
//    rewardPoints.textContent += " items";
//    infoCol.appendChild(productName);
//    infoCol.appendChild(size);
//    infoCol.appendChild(rewardPoints);

//    var priceCol = document.createElement("div");
//    priceCol.className = "col-lg-2 col-md-4 col-sm-6";
//    var itemPrice = document.createElement("h5");
//    itemPrice.className = "text-black";
//    //if (User.Identity.IsAuthenticated) {
//    //    itemPrice.textContent = product.ItemPrice.toFixed(2);
//    //}
//    priceCol.appendChild(itemPrice);

//    row.appendChild(imageCol);
//    row.appendChild(infoCol);
//    row.appendChild(priceCol);

//    card.appendChild(row);

//    return card;
//}


//// Render the CartVM data on the page
//function renderCart(ProductList) {
//    var container = document.getElementById("cartDetailsContainer");

//    if (ProductList.length > 0) {
//        ProductList.forEach(function (product) {
//            var productCard = createProductCard(product);
//            container.appendChild(productCard);
//        });

//        //if (User.Identity.IsAuthenticated) {
//        //    var subtotalAndTotalCard = createSubtotalAndTotal(cartVM.TotalPrice);
//        //    container.appendChild(subtotalAndTotalCard);
//        //}
//    }
//}


// Function to create a product card element
function createProductCard(product) {
    console.log(product);
    var card = $("<div>").addClass("card shadow p-3 col-sm-12 col-lg-7  mx-auto mt-3 rounded-3");
    var row = $("<div>").addClass("d-flex justify-content-between");

    var imageCol = $("<div>").addClass("col-3");
    var image = $("<img>").attr("src", product.croppedUrl).css({ "width": "90px", "border-radius": "50%" });
    imageCol.append(image);

    var infoCol = $("<div>").addClass("col px-2");
    var productName = $("<h4>").addClass("text-capitalize").text(product.name);
    var size = $("<p>").text("Grande");
    var rewardPoints = $("<p>").text(product.rewardPoints + " ");
    var starIcon = $("<i>").addClass("fa-solid fa-star fa-2xs");
    rewardPoints.append(starIcon);
    rewardPoints.append(" items");
    infoCol.append(productName, size, rewardPoints);

    var priceCol = $("<div>").addClass("col-2 text-end");
    var itemPrice = $("<h5>").addClass("text-black");

    if (product.itemPrice != null && product.itemPrice != 0) {
        itemPrice.text(product.itemPrice.toFixed(2));
    }

    priceCol.append(itemPrice);

    row.append(imageCol, infoCol, priceCol);
    card.append(row);

    return card;
}

// Function to create subtotal and total elements
function createSubtotalAndTotal(totalPrice) {
    var card = $("<div>").addClass("card shadow p-3 mb-3 col-lg-6 col-sm-12 mx-auto mt-3 rounded-3");
    var subtotalRow = $("<div>").addClass("row mb-3");

    var subtotalColLeft = $("<div>").addClass("col-lg-6 text-left");
    var subtotalHeader = $("<h5>").addClass("text-capitalize").text("Subtotal");
    subtotalColLeft.append(subtotalHeader);

    var subtotalColRight = $("<div>").addClass("col-lg-6 text-end");
    var subtotalAmount = $("<h5>").text(totalPrice.toFixed(2));
    subtotalColRight.append(subtotalAmount);

    subtotalRow.append(subtotalColLeft, subtotalColRight);

    var totalRow = $("<div>").addClass("row");

    var totalColLeft = $("<div>").addClass("col-lg-6 col-sm-6 text-left");
    var totalHeader = $("<h3>").addClass("text-black text-capitalize").text("Total");
    totalColLeft.append(totalHeader);

    var totalColRight = $("<div>").addClass("col-lg-6 col-sm-6 text-end");
    var totalAmount = $("<h3>").addClass("text-black").text(totalPrice.toFixed(2));
    totalColRight.append(totalAmount);

    totalRow.append(totalColLeft, totalColRight);

    card.append(subtotalRow, totalRow);

    return card;
}

// Render the CartVM data on the page
function renderCart(ProductList) {
    var container = $("#cartDetailsContainer");

    if (ProductList.length > 0) {
        $.each(ProductList, function (index, product) {
            var productCard = createProductCard(product);
            container.append(productCard);
        });

        //if (User.Identity.IsAuthenticated) {
        //    var subtotalAndTotalCard = createSubtotalAndTotal(jsonData.TotalPrice);
        //    container.append(subtotalAndTotalCard);
        //}
    }
}


function checkOut() {

}

