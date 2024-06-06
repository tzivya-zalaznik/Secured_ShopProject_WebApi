
const getAllProducts = async () => {
    console.log("ppp")
    const responseGet = await fetch('api/Product');
    if (responseGet.ok) {
        addToBasketIcon()
        const data = await responseGet.json();
        console.log('products ', data)
        drawProducts(data);
        const minPrice = Math.min(...data.map(item => item.price));
        const maxPrice = Math.max(...data.map(item => item.price));

        const minPriceInput = document.getElementById('minPrice');
        const maxPriceInput = document.getElementById('maxPrice');

        minPriceInput.value = minPrice;
        maxPriceInput.value = maxPrice;

        document.getElementById("counter").textContent = data.length;
    }
    else {
        alert("משהו השתבש")
    }
}

const drawProducts = (products) => {

    const template = document.getElementById('temp-card');

    products.forEach(product => {
        const card = template.content.cloneNode(true);

        card.querySelector('h1').textContent = product.name;
        card.querySelector('.price').textContent = product.price;
        card.querySelector('.description').textContent = product.description;
        card.querySelector('img').src = '../Images/' + product.imageUrl;
        card.querySelector('button').addEventListener('click', () => { addToBasket(product) });


        document.getElementById("PoductList").appendChild(card);
    });
}

const addToBasketIcon = () => {
    const productsArray = JSON.parse(sessionStorage.getItem('basket')) || [];
    let count = 0
    productsArray.forEach(item=>count+=1*item.quantity)
    if (productsArray) {
        document.getElementById("ItemsCountText").textContent = count;
    }
}

const addToBasket = (product) => {
    const productsArray = JSON.parse(sessionStorage.getItem('basket')) || [];
    const productIndex = productsArray.findIndex(item => item.product.productId === product.productId);
    if (productIndex !== -1) {
        productsArray[productIndex].quantity += 1;
    }
    else {
        productsArray.push({ product: product, quantity: 1 });
    }
    sessionStorage.setItem('basket', JSON.stringify(productsArray));
    addToBasketIcon()
}

const getAllCategories = async () => {
    const responseGet = await fetch('api/Category');
    if (responseGet.ok) {
        const data = await responseGet.json();
        console.log('categories ', data)
        drawCategories(data);
    }
    else {
        alert("משהו השתבש")
    }
}

const drawCategories = (categories) => {

    const template = document.getElementById('temp-category');

    categories.forEach(category => {
        const card = template.content.cloneNode(true);

        card.querySelector('.opt').id = category.categoryId;
        card.querySelector('.opt').value = category.categoryName;
        card.querySelector('.OptionName').textContent = category.categoryName;
        card.querySelector(".opt").addEventListener("change", (e) => { addTOFilterCategory(e,category) });
        document.getElementById("categoryList").appendChild(card);
    });
}

const removeBasket = () => {
    sessionStorage.removeItem('basket');
}

let categoryForFilter = []

const addTOFilterCategory = (e,category) => {
    if (e.target.checked) {
        categoryForFilter.push(category.categoryId);    
    } else {
        const index = categoryForFilter.indexOf(category.categoryId);
        categoryForFilter.splice(index, 1);
    }
    filterProducts()
}


const filterProducts = async () => {
    const minPrice = document.getElementById('minPrice').value
    const maxPrice = document.getElementById('maxPrice').value
    const description = document.getElementById('nameSearch').value
    let categories = ''
    categoryForFilter.forEach(c => categories+=`&category=${c}`)
    const responseGet = await fetch(`api/product?minPrice=${minPrice}&maxPrice=${maxPrice}&description=${description}${categories}`);

    if (responseGet.ok) {

        const data = await responseGet.json()
        document.getElementById('PoductList').replaceChildren();
        drawProducts(data)

        document.getElementById("counter").textContent = data.length;
    }
    else {
        alert("משהו השתבש")
    }
}

getAllCategories()

getAllProducts()