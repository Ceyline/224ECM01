//document.addEventListener("DOMContentLoaded", function () {
//    // Lấy tất cả các phần tử chứa nút tăng/giảm (quantity hoặc quantity-box)
//    const quantityWrappers = document.querySelectorAll('.quantity, .quantity-box');

//    quantityWrappers.forEach(wrapper => {
//        const minusBtn = wrapper.querySelector('button:first-child');
//        const plusBtn = wrapper.querySelector('button:last-child');
//        const input = wrapper.querySelector('input');

//        if (minusBtn && plusBtn && input) {
//            minusBtn.addEventListener('click', () => {
//                let quantity = parseInt(input.value) || 1;
//                if (quantity > 1) {
//                    input.value = quantity - 1;
//                }
//            });

//            plusBtn.addEventListener('click', () => {
//                let quantity = parseInt(input.value) || 1;
//                input.value = quantity + 1;
//            });
//        }
//    });
//});


//document.addEventListener("DOMContentLoaded", function () {
//    const deleteButtons = document.querySelectorAll(".delete-btn");

//    deleteButtons.forEach(button => {
//        button.addEventListener("click", function () {
//            const cartItem = this.closest(".cart-item");
//            if (cartItem) {
//                cartItem.remove();
//            }
//        });
//    });
//    // Xử lý xóa sản phẩm với span .remove-item
//    const removeSpans = document.querySelectorAll(".remove-item");
//    removeSpans.forEach(span => {
//        span.addEventListener("click", function () {
//            const cartItem = this.closest(".cart-item");
//            if (cartItem) {
//                cartItem.remove();
//            }
//        });
//    });
//});

//function openCart() {
//    document.getElementById('sidebar-cart').classList.add('active');
//    document.getElementById('cart-overlay').classList.add('active');
//}

//function closeCart() {
//    document.getElementById('sidebar-cart').classList.remove('active');
//    document.getElementById('cart-overlay').classList.remove('active');
//}

///**Thanh toán */
//document.addEventListener('DOMContentLoaded', () => {
//    const checkoutButton = document.querySelector('.checkout'); // Chọn nút Thanh toán

//    checkoutButton.addEventListener('click', () => {
//        window.location.href = 'thanhtoan2.html'; // Chuyển hướng đến thanhtoan.html
//    });
//});

document.addEventListener("DOMContentLoaded", function () {
    // Lấy tất cả các phần tử chứa nút tăng/giảm (quantity hoặc quantity-box)
    const quantityWrappers = document.querySelectorAll('.quantity, .quantity-box');

    quantityWrappers.forEach(wrapper => {
        const minusBtn = wrapper.querySelector('button:first-child');
        const plusBtn = wrapper.querySelector('button:last-child');
        const input = wrapper.querySelector('input');

        if (minusBtn && plusBtn && input) {
            minusBtn.addEventListener('click', () => {
                let quantity = parseInt(input.value) || 1;
                if (quantity > 1) {
                    input.value = quantity - 1;
                    updateCartItem(minusBtn.closest('.cart-item'), 'update');
                }
            });

            plusBtn.addEventListener('click', () => {
                let quantity = parseInt(input.value) || 1;
                input.value = quantity + 1;
                updateCartItem(plusBtn.closest('.cart-item'), 'update');
            });

            input.addEventListener('change', () => {
                let quantity = parseInt(input.value) || 1;
                if (quantity < 1) {
                    quantity = 1;
                    input.value = 1;
                }
                updateCartItem(input.closest('.cart-item'), 'update');
            });

        }
    });

    // Xử lý xóa sản phẩm với span .remove-item
    const removeSpans = document.querySelectorAll(".remove-item");
    removeSpans.forEach(span => {
        span.addEventListener("click", function () {
            const cartItem = this.closest(".cart-item");
            if (cartItem) {
                const productId = cartItem.dataset.productId;

                // Hiển thị xác nhận xóa
                Swal.fire({
                    title: 'Bạn có chắc chắn?',
                    text: "Sản phẩm sẽ bị xóa khỏi giỏ hàng!",
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#d33',
                    cancelButtonColor: '#3085d6',
                    confirmButtonText: 'Xóa',
                    cancelButtonText: 'Hủy'
                }).then((result) => {
                    if (result.isConfirmed) {
                        // Nếu người dùng chọn "Xóa"
                        updateCartItem(cartItem, 'delete'); // Cập nhật server khi xóa sản phẩm
                        cartItem.remove(); // Xóa khỏi giao diện
                        Swal.fire(
                            'Đã xóa!',
                            'Sản phẩm đã được xóa khỏi giỏ hàng.',
                            'success'
                        );
                    }
                });
            }
        });
    });

    // Xử lý xóa sản phẩm với nút .delete-btn (nếu có)
    const deleteButtons = document.querySelectorAll(".delete-btn");
    deleteButtons.forEach(button => {
        button.addEventListener("click", function () {
            const cartItem = this.closest(".cart-item");
            if (cartItem) {
                const productId = cartItem.dataset.productId;

                // Hiển thị xác nhận xóa
                Swal.fire({
                    title: 'Bạn có chắc chắn?',
                    text: "Sản phẩm sẽ bị xóa khỏi giỏ hàng!",
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#d33',
                    cancelButtonColor: '#3085d6',
                    confirmButtonText: 'Xóa',
                    cancelButtonText: 'Hủy'
                }).then((result) => {
                    if (result.isConfirmed) {
                        // Nếu người dùng chọn "Xóa"
                        updateCartItem(cartItem, 'delete'); // Cập nhật server khi xóa sản phẩm
                        cartItem.remove(); // Xóa khỏi giao diện
                        Swal.fire(
                            'Đã xóa!',
                            'Sản phẩm đã được xóa khỏi giỏ hàng.',
                            'success'
                        );
                    }
                });
            }
        });
    });

    //const checkoutButton = document.querySelector('.checkout');

    //if (checkoutButton) {
    //    checkoutButton.addEventListener('click', function () {
    //        // Điều hướng đến trang Index.cshtml của view ThanhToan
    //        window.location.href = '@Url.Action("Index", "ThanhToan")';
    //    });
    //}
});

function updateCartItem(cartItem, action) {
    // Tìm phần tử cha có class "cart-item" nếu chưa phải
    if (!cartItem.classList.contains("cart-item")) {
        cartItem = cartItem.closest(".cart-item");
    }

    const input = cartItem.querySelector('input');
    const quantity = parseInt(input.value) || 1;
    const productId = parseInt(cartItem.dataset.productId);  // Lấy đúng data từ HTML

    // Kiểm tra xem productId có giá trị hay không
    console.log("Product ID: ", productId);
    console.log("Quantity: ", quantity);

    if (!productId) {
        alert("Không thể xác định sản phẩm!");
        return;
    }

    $.ajax({
        url: '/GioHang/UpdateCart',
        type: 'POST',
        data: {
            productId: productId,
            quantity: quantity,
            action: action
        },
        success: function (response) {
            if (response.success) {
                console.log("Thành công: " + response.message);
                updateCart();
            } else {
                console.error("Lỗi: " + response.message);
                alert('Không thể cập nhật/xóa sản phẩm: ' + response.message);
            }
        },
        error: function (xhr, status, error) {
            console.error("Lỗi từ server: ", error);
            alert('Không thể kết nối đến server: ' + xhr.responseText);
        }
    });
}




// Hàm cập nhật lại tổng giỏ hàng khi thay đổi số lượng hoặc xóa sản phẩm
function updateCart() {
    const cartItems = document.querySelectorAll('.cart-item');
    let total = 0;

    cartItems.forEach(item => {
        const quantity = parseInt(item.querySelector('input').value) || 1;
        const price = parseInt(item.querySelector('.item-price').textContent.replace(/[₫,]/g, '')) || 0;
        total += quantity * price;
    });

    // Cập nhật tổng giỏ hàng
    const checkoutButton = document.querySelector('.checkout-btn');
    if (checkoutButton) {
        checkoutButton.textContent = `THANH TOÁN ・ ${total.toLocaleString()}₫`;
    }
}
function openCart() {
    document.getElementById('sidebar-cart').classList.add('active');
    document.getElementById('cart-overlay').classList.add('active');
}

function closeCart() {
    document.getElementById('sidebar-cart').classList.remove('active');
    document.getElementById('cart-overlay').classList.remove('active');
}

