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
                }
            });

            plusBtn.addEventListener('click', () => {
                let quantity = parseInt(input.value) || 1;
                input.value = quantity + 1;
            });
        }
    });
});


document.addEventListener("DOMContentLoaded", function () {
    const deleteButtons = document.querySelectorAll(".delete-btn");

    deleteButtons.forEach(button => {
        button.addEventListener("click", function () {
            const cartItem = this.closest(".cart-item");
            if (cartItem) {
                cartItem.remove();
            }
        });
    });
    // Xử lý xóa sản phẩm với span .remove-item
    const removeSpans = document.querySelectorAll(".remove-item");
    removeSpans.forEach(span => {
        span.addEventListener("click", function () {
            const cartItem = this.closest(".cart-item");
            if (cartItem) {
                cartItem.remove();
            }
        });
    });
});

function openCart() {
    document.getElementById('sidebar-cart').classList.add('active');
    document.getElementById('cart-overlay').classList.add('active');
}

function closeCart() {
    document.getElementById('sidebar-cart').classList.remove('active');
    document.getElementById('cart-overlay').classList.remove('active');
}

/**Thanh toán */
document.addEventListener('DOMContentLoaded', () => {
    const checkoutButton = document.querySelector('.checkout'); // Chọn nút Thanh toán

    checkoutButton.addEventListener('click', () => {
        window.location.href = 'thanhtoan2.html'; // Chuyển hướng đến thanhtoan.html
    });
});