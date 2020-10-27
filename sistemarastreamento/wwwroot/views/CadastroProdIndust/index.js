var index = {
    btnCadastrar: function () {

        var id = document.getElementById("hfIdEditar").value;
        if (id == "")
            id = 0;

        var cod_ref = document.getElementById("cod_ref").value;
        var descricao = document.getElementById("descricao").value;

        var msgerror = document.getElementById("error");
        var msgsucess = document.getElementById("sucess");

        msgerror.innerHTML = "";
        msgerror.style.display = "none";

        msgsucess.innerHTML = "";
        msgsucess.style.display = "none";

        if (cod_ref.trim() == "") {
            msgerror.innerHTML = "Preencha o Código de Referência";
            if (msgsucess.style.display == "block")
                msgsucess.style.display = "none";

            msgerror.style.display = "block";
        }
        else if (descricao.trim() == "") {
            msgerror.innerHTML = "Preencha a Descrição";
            if (msgsucess.style.display == "block")
                msgsucess.style.display = "none";

            msgerror.style.display = "block";
        }
        else {
            var dados = {
                cod_ref,
                descricao
            }

            var config = {
                method: "POST",
                headers: {
                    "Content-Type": "application/json; charset=utf-8",
                },
                credentials: 'include', //inclui cookies
                body: JSON.stringify(dados)  //serializa
            };

            fetch("/CadastroProdIndust/Criar?id=" + id, config)
                .then(function (dadosJson) {
                    var obj = dadosJson.json();
                    return obj;
                })
                .then(function (dadosObj) {
                    if (dadosObj.operacao) {
                        if (id == 0) {
                            msgsucess.innerHTML = "Cadastro Realizado!";
                            if (msgerror.style.display == "block")
                                msgerror.style.display = "none";
                            msgsucess.style.display = "block";
                        }
                        else {
                            msgsucess.innerHTML = "Alteração Realizado!";
                            if (msgerror.style.display == "block")
                                msgerror.style.display = "none";
                            msgsucess.style.display = "block";

                            window.location.href = "/CadastroProdIndust/indexListar";
                        }
                        document.getElementById("cod_ref").value = "";
                        document.getElementById("descricao").value = "";
                    }
                })
                .catch(function (e) {
                    alert("deu erro");
                }) 
        }
    },

    obterDadosEditar: function (id) {

        var config = {
            method: "GET",
            headers: {
                "Content-Type": "application/json; charset=utf-8",
                "Accept": "application/json",
            },
            credentials: 'include', //inclui cookies
        };

        fetch("/CadastroProdIndust/ObterEditar?id=" + id, config)
            .then(function (dadosJson) {
                var obj = dadosJson.json(); //deserializando
                return obj;
            })
            .then(function (dadosObj) {

                document.getElementById("cod_ref").value = dadosObj.prodindust.cod_ref;
                document.getElementById("descricao").value = dadosObj.prodindust.descricao;

            })
            .catch(function () {

                alert("deu erro")
            })

    },
    limpaInfo: function () {
        document.getElementById("sucess").style.display = 'none';
    }
}

if (document.getElementById("hfIdEditar") != null) {
    if (document.getElementById("hfIdEditar").value != "") {
        index.obterDadosEditar(document.getElementById("hfIdEditar").value)
    }
}