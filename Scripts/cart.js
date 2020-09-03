const $cartBadge = $('.cart-badge');

function isValidURL(str) {
    var a = document.createElement('a');
    a.href = str;
    return (a.host && a.host != window.location.host);
}

/**** ACTIONS ****/

const addProduct = function (id, name, summary, price, quantity, image) {
    cart.push({
        id: id,
        name: name,
        description: summary,
        price: price,
        amount: quantity,
        image: image
    });

    drawTable();
    $.get('/Orders/AddItem', { accountID, productID: id, amount: quantity })
        .done(function () {
            $cartBadge.text(getTotalQuantity());
        })
        .fail(function (e) {
            console.log(e, " - add item failed");
        });
    
};

const addOneProduct = function (id, name, summary, price, image) {
    if (!isAValidProduct(id, name, summary, price, 1, image)) return false;
    summary = typeof summary === "undefined" ? "" : summary;

    const productIndex = getIndexOfProduct(id);

    if (productIndex < 0)
        addProduct(id, name, summary, price, 1, image);
    else
        updatePoduct(id, cart[productIndex].amount + 1);
}

const isAValidProduct = function (id, name, summary, price, quantity, image) {
    if (typeof id === "undefined") {
        console.error("id required");
        return false;
    }
    if (typeof name === "undefined") {
        console.error("name required");
        return false;
    }
    if (typeof image === "undefined") {
        console.error("image required");
        return false;
    }
    if (!$.isNumeric(price)) {
        console.error("price is not a number");
        return false;
    }
    if (!$.isNumeric(quantity)) {
        console.error("quantity is not a number");
        return false;
    }

    return true;
}

const setProduct = function (id, name, summary, price, quantity, image) {
    if (!isAValidProduct(id, name, summary, price, quantity, image)) return false;
    summary = typeof summary === "undefined" ? "" : summary;

    if (!updatePoduct(id)) {
        addProduct(id, name, summary, price, quantity, image);
    }
};

const removeProduct = function (id) {
    cart = $.grep(cart, function (value, index) {
        return value.id != id;
    });

    $.get('/Orders/UpdateItem', { accountID, productID: id, amount: 0 })
        .done(function () {
            $cartBadge.text(getTotalQuantity());
        })
        .fail(function (e) {
            console.log(e, " - update item failed");
        });
};

const updateCart = function () {
    $.each($(".product-quantity"), function () {
        var id = $(this).closest("tr").data("id");
        updatePoduct(id, $(this).val());
    });
};

const clearProducts = function () {
    cart = [];
}

const updatePoduct = function (id, quantity) {
    const productIndex = getIndexOfProduct(id);
    if (productIndex < 0) return false;

    cart[productIndex].amount = typeof quantity === "undefined" ? cart[productIndex].amount * 1 + 1 : quantity;

    drawTable();
    $.get('/Orders/UpdateItem', { accountID, productID: id, amount: quantity })
        .done(function () {
            $cartBadge.text(getTotalQuantity());
        })
        .fail(function (e) {
            console.log(e, " - update item failed");
        });
    return true;
};

/**** GETTERS ****/

const getTotalQuantity = function () {
    var total = 0;

    $.each(cart, function (index, value) {
        total += value.amount * 1;
    });
    return total;
};

const getIndexOfProduct = function (id) {
    let productIndex = -1;

    $.each(cart, function (index, value) {
        if (value.id == id) {
            productIndex = index;
            return;
        }
    });

    return productIndex;
};

const getTotalPrice = function () {
    let total = 0;

    $.each(cart, function (index, value) {
        total += value.amount * value.price;
        total *= 1;
    });

    return total;
};

/**** EVENTS ****/

$(document).on('click', ".product-remove", function () {
    var $tr = $(this).closest("tr");
    var id = $tr.data("id");
    $tr.hide(500, function () {
        removeProduct(id);
        drawTable();
        $cartBadge.text(getTotalQuantity());
    });
});

$(document).on("input", ".product-quantity", function () {
    var price = $(this).closest("tr").data("price");
    var id = $(this).closest("tr").data("id");
    var quantity = $(this).val();

    $(this).parent("td").next(".product-quantity").text("$" + (price * quantity));
    updatePoduct(id, quantity);

    $cartBadge.text(getTotalQuantity());
    showGrandTotal();
});

/**** DRAW ****/

const showGrandTotal = function () {
    $("#cart-grand-total").text("$" + getTotalPrice());
};

const drawTable = function () {
    const $cartTable = $("#cart-table");
    $cartTable.empty();

    $.each(cart, function () {
        let total = this.amount * this.price;
        $cartTable.append(`
                <tr title="${this.name}" data-id="${this.id}" data-price="${this.price}">
                    <td class="text-center" style="width: 50px;"><img width="50px" height="50px" src="${isValidURL(this.image) ? this.image : `/Media/Images/${this.image}`}"/></td>
                    <td>${this.name}</td>
                    <td title="Unit Price" class="text-right">$${this.price}</td>
                    <td title="Quantity"><input type="number" min="1" style="width: 70px;" class="product-quantity" value="${this.amount}"/></td>
                    <td title="Total" class="text-right product-total">$${total}</td>
                    <td title="Remove from Cart" class="text-center" style="width: 30px;">
                        <a href="javascript:void(0);" class="btn btn-xs btn-danger product-remove">X</a>
                    </td>
                </tr>
            `);
    });

    $cartTable.append(cart.length ? `
            <tr>
                <td></td>
                <td><strong>Total</strong></td>
                <td></td>
                <td></td>
                <td class="text-right"><strong id="cart-grand-total"></strong></td>
                <td></td>
            </tr>` :
        '<div class="alert alert-danger" role="alert" id="cart-empty-message">Your cart is empty</div>'
    );


    showGrandTotal();
};

drawTable();
$cartBadge.text(getTotalQuantity());