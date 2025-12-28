// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

const validIconList = [
    'question',
    'backpack',
    'bag',
    'bag-heart',
    'airplane',
    'alphabet',
    'bandaid',
    'capsule',
    'box-seam',
    'buildings',
    'cake',
    'car-front',
    'cart',
    'credit-card',
    'cpu',
    'currency-bitcoin',
    'earbuds',
    'fuel-pump',
    'gem',
    'gift',
    'globe-americas',
    'keyboard',
    'piggy-bank',
    'house',
    'person-fill',
    'receipt',
    'balloon'
];


// Write your JavaScript code.
document.addEventListener('DOMContentLoaded', () => {
    setBaseValidation();
    setNavigationMenu();
});

function setBaseValidation() {
    const v = new aspnetValidation.ValidationService();
    v.ValidationInputCssClassName = 'is-invalid';
    v.ValidationInputValidCssClassName = 'is-valid';
    v.ValidationMessageCssClassName = 'invalid-feedback';
    v.ValidationMessageValidCssClassName = 'valid-feedback';

    v.addProvider('daterange', (value, element, params) => {
        if (!value) {
            return true;
        }

        let valueDate = new Date(value);
        let minDate = new Date(params.min);
        let maxDate = new Date(params.max);

        return (minDate < valueDate && valueDate < maxDate);
    });

    v.bootstrap();
}

function setNavigationMenu() {
    new bootstrap.Dropdown(document.querySelector('.dropdown-toggle'), {
        popperConfig: {
            strategy: 'fixed',
            modifiers: [
                { name: 'preventOverflow', options: { boundary: 'viewport' } }
            ]
        }
    });
}

function initializeCurrencyInput(input) {
    input.value = input.value == '' ? '$0.00' : input.value;

    // Primary action for the setup, allows for typing currency from right -> left
    input.addEventListener('input', function (event) {
        // Gets the initial value
        let value = event.target.value

        // Replace and format; remove all non-numbers
        value = value.replace(/[^0-9]/g, '');

        // Format as currency
        if (value) {
            const numberValue = Number.parseFloat(value) / 100;

            event.target.value = new Intl.NumberFormat('en-US', {
                style: 'currency',
                currency: 'USD',
                minimumFractionDigits: 2,
                maximumFractionDigits: 2
            }).format(numberValue);
        }
    });

    input.addEventListener('keydown', function (event) {
        if (!/[0-9]/.test(event.key) && !(event.key == 'Backspace') && !(event.key == 'Tab')) {
            event.preventDefault();
        }
    })

    input.addEventListener('focus', function () {
        this.setSelectionRange(this.value.length, this.value.length);
    });

    input.addEventListener('click', e => e.preventDefault());
    input.addEventListener('select', e => e.preventDefault());
    input.addEventListener('mouseup', e => e.preventDefault());
}

function showToast(message) {
    const pageToast = bootstrap.Toast.getOrCreateInstance('#toast-page');
    const body = pageToast._element.querySelector('.toast-body');

    // Set and show
    body.textContent = message;
    pageToast.show();
}
