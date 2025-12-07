// ----- swiper initializing -----
var swiper = new Swiper(".newSwiper", {
    loop: true,
    centeredSlides: false,
    slidesPerView: 1,
    spaceBetween: 5,

    autoplay: {
        delay: 2500,
        disableOnInteraction: false,
    },

    navigation: {
        nextEl: ".pw-new-next",
        prevEl: ".pw-new-prev",
    },

    breakpoints: {
        768: {
            slidesPerView: 2,
            spaceBetween: 50,
            centeredSlides: false,
        },
        1200: {
            slidesPerView: 3,
            spaceBetween: 70,
            centeredSlides: false,
        },
    },
});

// -------- şifre açma ve gizleme Jquery ile --------
$(".fa-eye").on("click", function () {
    const $inputField = $(this).closest(".position-relative").find("input");
    if ($inputField.attr("type") === "password") {
        $inputField.attr("type", "text");
    } else {
        $inputField.attr("type", "password");
    }
});

// ----- kategori ekleme -----
const uploadContainer1 = document.getElementById("upload-container1");
const fileInput1 = document.getElementById("file-input1");

uploadContainer1?.addEventListener("click", () => {
    fileInput1?.click();
});

fileInput1?.addEventListener("change", (event) => {
    const file1 = event.target.files[0];
    if (file1) {
        const reader1 = new FileReader();
        reader1.onload = (e) => {
            uploadContainer1.innerHTML = `<img src="${e.target.result} " alt="Uploaded Image">`;
        };
        reader1.readAsDataURL(file1);
    }
});

// ---- Kategori düzeltme ----
const uploadContainer4 = document.getElementById("upload-container4");
const fileInput4 = document.getElementById("file-input4");

fileInput4?.addEventListener("change", (event) => {
    const file4 = event.target.files[0];
    if (file4) {
        const reader4 = new FileReader();
        reader4.onload = (e) => {
            uploadContainer4.innerHTML = `<img src="${e.target.result}" alt="Uploaded Image" style="max-width: 100%;">`;
        };
        reader4.readAsDataURL(file4);
    }
});

// ----- kitap ekleme -----
const uploadContainer2 = document.getElementById("upload-container2");
const fileInput2 = document.getElementById("file-input2");

uploadContainer2?.addEventListener("click", () => {
    fileInput2?.click();
});

fileInput2?.addEventListener("change", (event) => {
    const file2 = event.target.files[0];
    if (file2) {
        const reader2 = new FileReader();
        reader2.onload = (e) => {
            uploadContainer2.innerHTML = `<img src="${e.target.result}" alt="Uploaded Image">`;
        };
        reader2.readAsDataURL(file2);
    }
});

const dropdownItems = document.querySelectorAll(".dropdown-item");
const selectedCategoryInput = document.getElementById("selectedCategoryInput");
const categoryDropdownButton = document.querySelector(".btn-dropdown span.text-start");

dropdownItems.forEach(item => {
    item.addEventListener("click", function (event) {
        const selectedCategoryId = this.getAttribute("data-value");
        selectedCategoryInput.value = selectedCategoryId;

        categoryDropdownButton.textContent = this.textContent;
    });
});

// ----- kitap düzeltme ------
const uploadContainer = document.getElementById("upload-container8");
const fileInput = document.getElementById("file-input");

fileInput?.addEventListener("change", (event) => {
    const file = event.target.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = (e) => {
            uploadContainer.innerHTML = `<img src="${e.target.result}" alt="Uploaded Image" style="max-width: 100%;">`;
        };
        reader.readAsDataURL(file);
    }
});

// -------- şifre açma ve gizleme --------
const eyes3 = document.querySelectorAll(".fa-eye");
eyes3.forEach((eyeIcon) => {
    eyeIcon.addEventListener("click", function () {
        const inputField =
            this.closest(".position-relative")?.querySelector("input");
        if (inputField?.type === "password") {
            inputField.type = "text";
        } else {
            inputField.type = "password";
        }
    });
});

// -------- şifre açma ve gizleme --------
const eyes2 = document.querySelectorAll(".fa-eye");
eyes2.forEach((eyeIcon) => {
    eyeIcon.addEventListener("click", function () {
        const inputField =
            this.closest(".position-relative")?.querySelector("input");
        if (inputField?.type === "password") {
            inputField.type = "text";
        } else {
            inputField.type = "password";
        }
    });
});

// ------ sidebar'daki kategori ve kitap listlerini açma ve kapama ------
const btnKategori = document.querySelector(".btn-kategori");
const kategoriList = document.querySelector(".kategori-list");

btnKategori?.addEventListener("click", function () {
    kategoriList.classList.toggle("d-none");
});

const btnKitap = document.querySelector(".btn-kitap");
const kitapList = document.querySelector(".kitap-list");

btnKitap?.addEventListener("click", function () {
    kitapList.classList.toggle("d-none");
});
