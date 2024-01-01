
$(document).ready(function () {

    $('#addToOrderBtn').click(function () {

        var productObj = new Object();
        productObj.ProductId = $("#Product_ProductId").val();
        productObj.ProductName = $("#Product_Name").val();
      
        objArray.push(productObj);

        var objValue = JSON.stringify(objArray);
       

        document.getElementById("cartCount").innerHTML = objArray.length;

        //console.log(objArray);

        localStorage.setItem("objKey", objValue);
    });
});



