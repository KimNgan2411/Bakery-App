
//product search form
var pageNumEl = $('#product-page-number');
function searchProduct() {
    $('#product-frm').submit();
}
//end product search form

//slider
var sliderCount = $('.slider-item').length;
var sliderInd = 0;
const sliderLim = $('.slider-container').data('limit');

$('.slider-container .prev-slide').on('click', () => {
    moveSlide(-1);
})
$('.slider-container .next-slide').on('click', () => {
    moveSlide(1);
})

function moveSlide(i) {
    if (sliderCount <= sliderLim) return;

    var sliderContainer = document.querySelector('.slider-container');
    var width = getComputedStyle(sliderContainer).getPropertyValue('--slide-width');
    sliderInd += i;
    if (sliderInd < 0) sliderInd = sliderCount - sliderLim;
    if (sliderInd > sliderCount - sliderLim) sliderInd = 0;
    $('.slider').css('transform', `translateX(calc( ${-sliderInd} * ${width}))`);
}
//end slider

//dialog
$('.dialog-wrapper').on('click', (e) => {
    if (e.target == e.currentTarget) {
        $(e.target).addClass('hidden');
    }
})
//end dialog

//numeric input
function onQuantityKeydown(event) {
    var key = event.keyCode;
    //accept only backspace, 0-9, arrows, del
    if (key != 8 && (key < 48 || key > 57) && (key < 37 || key > 40) && key != 46) {
        event.preventDefault();
        return false;
    }
}

function onCartQuantityChange(event, masp) {
    let val = event.target.value;
    val = val.replace(/[^0-9]/g, '');
    if (isNaN(val) || val == "") val = 1;
    if (parseInt(val) > parseInt(event.target.max)) val = event.target.max;

    if (val != event.target.value) event.target.value = val;

    $.post('/giohangs/UpdateQuantity', { masp, sl: event.target.value }).fail((xhr, status, error) => {
        alert('Số lượng sản phẩm có thay đổi')
        window.location.reload();
    })
}

function onQuantityChange(event) {
	let val = event.target.value;
	if (isNaN(val) || val == "") event.target.value = 1;
}
//end numeric input

//show toast
(function showToast() {
    var toastElList = [].slice.call(document.querySelectorAll('.toast'))
    var toastList = toastElList.map(function (toastEl) {
        return new bootstrap.Toast(toastEl)
    })
    toastList.forEach(toast => toast.show())
})();