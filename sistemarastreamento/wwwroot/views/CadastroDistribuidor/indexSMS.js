var index = {
    btnEnviar: function () {
        var telefone = document.getElementById('telefone').value
        var mensagem = document.getElementById('mensagem').value
        document.getElementById('btnEnviar').value = 'Enviando...'
        var dados = {
            telefone,
            mensagem
        }

        var config = {
            method: "POST",
            headers: {
                "Content-Type": "application/json; charset=utf-8",
            },
            credentials: 'include', //inclui cookies
            body: JSON.stringify(dados)  //serializa
        };

        fetch("/CadastroDistribuidor/EnviarSMS", config)
            .then(function (dadosJson) {
                var obj = dadosJson.json(); //deserializando
                return obj;
            })
            .then(function (dadosObj) {
                if (dadosObj.operacao) {
                    window.parent.$.fancybox.close();
                }

            })
            .catch(function (e) {
                alert("deu erro");
            })
    }
}