
const register = async () => {
    const postData = {
        firstName: document.getElementById("firstName").value,
        lastName: document.getElementById("lastName").value,
        email: document.getElementById("userName").value,
        password: document.getElementById("password").value
    }
    if (postData.firstName == '' || postData.lastName == '' || postData.email == '' || postData.password == '') {
        alert('כל השדות חובה')
    }
    else {
        const strength = await checkPassword();
        if (strength >= 3) {
            const responsePost = await fetch('api/user', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(postData)
            });
            if (responsePost.ok) {
                const dataPost = await responsePost.json();
                window.location.href = "login.html";
            }
            else {
                alert("אופס, אחד או יותר מן הנתונים שגוי...")
            }
        }
        else {
            alert("סיסמא חלשה");
        }
    }
}

const login = async () => {
    const postData = {
        email: document.getElementById("userName").value,
        password: document.getElementById("password").value
    }
    const responsePost = await fetch('api/user/login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(postData)
    });
    if (responsePost.status == 204) {
        alert("משתמש לא קיים")
    }
    else {
        if (responsePost.ok) {
            const dataPost = await responsePost.json();
            sessionStorage.setItem("user", JSON.stringify(dataPost));
            window.location.href = "home.html";
        }
        else {
            alert("שם משתמש או סיסמא אינם תקינים")
        }
    }
}

const update = async () => {
    const postData = {
        firstName: document.getElementById("firstName").value,
        lastName: document.getElementById("lastName").value,
        email: document.getElementById("userName").value,
        password: document.getElementById("password").value
    }
    if (postData.firstName == '' || postData.lastName == '' || postData.email == '' || postData.password == '') {
        alert('כל השדות חובה')
    }
    else {
        const strength = await checkPassword();
        if (strength >= 3) {
            const responsePost = await fetch(`api/user/${JSON.parse(sessionStorage.getItem("user")).userId}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(postData)
            });
            if (responsePost.ok) {
                alert("הפרטים עודכנו בהצלחה");
                window.location.href = "login.html";
            }
            else {
                alert("אופס, אחד או יותר מן הנתונים שגוי...")
            }
        }
        else {
            alert("סיסמא חלשה");
        }
    }
}

const checkPassword = async () => {

    var strength = {
        0: "Worst",
        1: "Bad",
        2: "Weak",
        3: "Good",
        4: "Strong"
    }
    var password = document.getElementById("password").value;
    var progress = document.getElementById("password-strength-progress");
    var text = document.getElementById('password-strength-text');

    const responsePost = await fetch('api/user/checkPassword', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(password)
    });
    const dataPost = await responsePost.json();

    progress.value = dataPost

    if (password !== "") {
        text.innerHTML = "Strength: " + strength[dataPost];
    } else {
        text.innerHTML = "";
    }
    return parseInt(dataPost);
}