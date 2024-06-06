
const GetAllProducts = () => {
    const productsArr = JSON.parse(sessionStorage.getItem("basket"));
    drawBasket(productsArr)
    totalSum(productsArr)
}

const totalSum = (productsArr) => {
    let sum = 0
    let count = 0
    productsArr.forEach(p => { sum += (p.product.price) * p.quantity; count += 1 * p.quantity })
    document.getElementById('itemCount').innerHTML = count
    document.getElementById('totalAmount').innerHTML = sum
}

const drawBasket = (productsArr) => {
        const template = document.getElementById('temp-row');

        productsArr.forEach(item => {
            const row = template.content.cloneNode(true);
            row.querySelector(".price").innerText = item.product.price * item.quantity;
            row.querySelector(".image").src = '../Images/' + item.product.imageUrl;
            row.querySelector(".descriptionColumn").innerText = item.product.description;
            row.querySelector(".quantity").innerText = item.quantity
            row.querySelector(".DeleteButton").addEventListener('click', () => { removeFromBasket(item.product) });
            row.querySelector(".plus").addEventListener('click', () => { plus(item.product) });
            row.querySelector(".minus").addEventListener('click', () => { minus(item.product) });

            document.getElementById("itemList").appendChild(row); 
            });
}

const plus = (product) => {
    const productsArray = JSON.parse(sessionStorage.getItem('basket'));
    const productIndex = productsArray.findIndex(item => item.product.productId === product.productId);
    productsArray[productIndex].quantity += 1;
    sessionStorage.setItem('basket', JSON.stringify(productsArray));
    document.getElementById('itemList').replaceChildren();
    drawBasket(productsArray)
    totalSum(productsArray)
}

const minus = (product) => {
    const productsArray = JSON.parse(sessionStorage.getItem('basket'));
    const productIndex = productsArray.findIndex(item => item.product.productId === product.productId);
    if (productsArray[productIndex].quantity > 1) {
        productsArray[productIndex].quantity -= 1;
        sessionStorage.setItem('basket', JSON.stringify(productsArray));
        document.getElementById('itemList').replaceChildren();
        drawBasket(productsArray)
        totalSum(productsArray)
    }
    else {
        removeFromBasket(product)
    }
}

const removeFromBasket = (product) => {
    const productsArr = JSON.parse(sessionStorage.getItem("basket"));
    const productsToDraw = productsArr.filter(p => p.product.productId != product.productId);
    sessionStorage.setItem('basket', JSON.stringify(productsToDraw))
    document.getElementById('itemList').replaceChildren();
    drawBasket(productsToDraw)
    totalSum(productsToDraw)
}

const afterOrder = (dataPost) => {
    sessionStorage.removeItem('basket')
    alert(`Order ${dataPost.orderId} successfully sent, total to be paid: ${dataPost.orderSum}₪`)
    window.location.href = "Products.html";
}

const placeOrder = async () => {

    if (!sessionStorage.getItem('user')) {
        alert("you must login...")
        return
    }

    let orderItemsArray = [];
    const productsArray = JSON.parse(sessionStorage.getItem('basket'))
    if (!productsArray || productsArray.length == 0) {
        alert("there is no item...")
        return
    }

    for (let i = 0; i < productsArray.length; i++) {
        orderItem = {
            ProductId: productsArray[i].product.productId,
            quantity: productsArray[i].quantity
        }
        orderItemsArray.push(orderItem);
    }

    const postData = {
        orderDate: new Date(),
        orderSum: parseFloat(document.getElementById('totalAmount').innerHTML),
        userId: JSON.parse(sessionStorage.getItem('user')).userId,
        orderItems: orderItemsArray
    }

    const responsePost = await fetch('api/Order', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(postData)
    });
    if (responsePost.ok) {
        const dataPost = await responsePost.json();
        console.log('Poast Data ', dataPost)
        afterOrder(dataPost)
    }
    else {
        alert("אופס, אחד או יותר מן הנתונים שגוי...")
    }
}

 GetAllProducts()