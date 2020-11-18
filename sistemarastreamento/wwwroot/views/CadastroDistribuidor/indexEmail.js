var index = {
    btnEnviar: function () {
        var destino = document.getElementById('destino').value
        var origem = document.getElementById('origem').value
        var mensagem = document.getElementById('mensagem').value
        var assunto = document.getElementById('assunto').value
        document.getElementById('btnEnviar').value = 'Enviando...'
        var dados = {
            destino,
            origem,
            assunto,
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

        fetch("/CadastroDistribuidor/EnviarEmail", config)
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