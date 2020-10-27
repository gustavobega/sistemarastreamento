const inputs = document.querySelectorAll(".input");

function addcl() {
    let parent = this.parentNode.parentNode;
    parent.classList.add("focus");
}

function remcl() {
    let parent = this.parentNode.parentNode;
    if (this.value == "") {
        parent.classList.remove("focus");
    }
}

inputs.forEach(input => {
    input.addEventListener("focus", addcl);
    input.addEventListener("blur", remcl);
});

var index = {
    btnLogar: function () {

        document.getElementById("input_img").style.display = "block";
        document.getElementById("btnLogar").disabled = true;
        document.getElementById("btnLogar").style.background = "#5092c8";

        var email = document.getElementById("email").value;
        var senha = document.getElementById("senha").value;
        var tipo = document.getElementById("tipo").value;

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
                    alert("Algo deu errado, tente novamente!");
                    document.getElementById("btnLogar").disabled = "";
                }
                
            })
            .catch(function (e) {
                alert("Algo deu errado, tente novamente!");
            })
            .finally(function () {
                document.getElementById("btnLogar").disabled = false;
            });
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