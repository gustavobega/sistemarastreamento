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

        var email = document.getElementById("email").value;
        var senha = document.getElementById("senha").value;
        var industria = document.getElementById("industria");
        var tipo = ""

        if (industria.checked)
            tipo = 'Indústria'
        else
            tipo = 'Distribuidor'

        var checkemail = index.validacaoEmail(email);

        if (email.trim() == "") {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Preencha o E-Mail!'
            })
        }
        else if (!checkemail) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Preencha o E-Mail inválido!'
            })
        }
        else if (senha.trim() == "") {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Preencha a Senha!'
            })
        }
        else {
            document.getElementById("btnLogar").value = 'aguarde...';
            document.getElementById("btnLogar").disabled = true;

            var dados = {
                email,
                senha,
                tipo
            }
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
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: 'Sessão Inválida!'
                        })
                        document.getElementById("btnLogar").disabled = "";
                        document.getElementById("btnLogar").value = 'Entrar';
                    }

                })
                .catch(function (e) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'Algo deu Errado, Tente Novamente!'
                    })
                })

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