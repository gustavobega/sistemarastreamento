const sign_in_btn = document.querySelector("#sign-in-btn");
const sign_up_btn = document.querySelector("#sign-up-btn");
const container = document.querySelector(".container");

sign_up_btn.addEventListener("click", () => {
    container.classList.add("sign-up-mode");
});

sign_in_btn.addEventListener("click", () => {
    container.classList.remove("sign-up-mode");
});

var index = {

    validacaoEmail: function (field) {
        usuario = field.substring(0, field.indexOf("@"));
        dominio = field.substring(field.indexOf("@") + 1, field.length);

        if ((usuario.length >= 1) &&
            (dominio.length >= 3) &&
            (usuario.search("@") == -1) &&
            (dominio.search("@") == -1) &&
            (usuario.search(" ") == -1) &&
            (dominio.search(" ") == -1) &&
            (dominio.search(".") != -1) &&
            (dominio.indexOf(".") >= 1) &&
            (dominio.lastIndexOf(".") < dominio.length - 1)) {
            return true;
        }
        else {
            return false;
        }
    },

    btnLogar: function () {

        document.getElementById("btnLogar").style.background = "#5092c8";

        var email = document.getElementById("email").value;
        var senha = document.getElementById("senha").value;
        var tipo = document.getElementById("tipo").value;

        var error = document.getElementById('error')

        var checkemail = index.validacaoEmail(email);

        if (email.trim() == "") {
            error.innerHTML = "Preencha o E-mail"
            error.style.display = 'block'
        }
        else if (!checkemail) {
            error.innerHTML = "E-mail inválido"
            error.style.display = 'block'
        }
        else if (senha.trim() == "") {
            error.innerHTML = "Preencha a Senha"
            error.style.display = 'block'
        }
        else {

            document.getElementById("btnLogar").disabled = true;

            var dados = {
                email,
                senha,
                tipo
            }

            document.getElementById("gif-login").style.display = 'block';
            var config = {
                method: "POST",
                headers: {
                    "Content-Type": "application/json; charset=utf-8"
                },
                credentials: 'include', //inclui cookies
                body: JSON.stringify(dados)  //serializa
            };

            fetch("/Login/Validar", config)
                .then(function (dadosJson) {
                    var obj = dadosJson.json(); //deserializando
                    return obj;
                })
                .then(function (dadosObj) {
                    if (dadosObj.operacao) {
                        window.location.href = "/Default";
                    }
                    else {
                        error.innerHTML = "Sessão Inválida!"
                        error.style.display = 'block'
                        document.getElementById("btnLogar").disabled = "";
                        document.getElementById("gif-login").style.display = 'none';
                    }

                })
                .catch(function (e) {
                    error.innerHTML = "Algo deu Errado, Tente Novamente!"
                    error.style.display = 'block'
                    document.getElementById("gif-login").style.display = 'none';
                })
                .finally(function () {
                    document.getElementById("btnLogar").disabled = false;
                });

        }
       
    },

    direcionaIndustria: function () {
        $.fancybox.close();
        window.location.href = "/CadastroIndustria/index";
    },

    direcionaDistribuidor: function () {
        $.fancybox.close();
        window.location.href = "/CadastroDistribuidor/index";
    }
}

document.getElementById("email")
    .addEventListener("keyup", function (event) {
        event.preventDefault();
        if (event.keyCode === 13) {
            index.btnLogar();
        }
    });

document.getElementById("senha")
    .addEventListener("keyup", function (event) {
        event.preventDefault();
        if (event.keyCode === 13) {
            index.btnLogar();
        }
    });