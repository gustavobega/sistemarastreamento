var index = {
    btnCadastrar: function () {

        var id = document.getElementById("hfIdEditar").value;
        if (id == "")
            id = 0;

        var cod_ref = document.getElementById("cod_ref").value;
        var codigo = document.getElementById("codigo").value;
        var lote = document.getElementById("lote").value;
        var saldo = document.getElementById("saldo").value;

        var msgerror = document.getElementById("error");
        var msgsucess = document.getElementById("sucess");

        msgerror.innerHTML = "";
        msgerror.style.display = "none";

        msgsucess.innerHTML = "";
        msgsucess.style.display = "none";

        if (cod_ref.trim() == "") {
            msgerror.innerHTML = "Preencha o Código de Referência";
            document.getElementById("error2").style.display = "none";
            if (msgsucess.style.display == "block")
                msgsucess.style.display = "none";

            msgerror.style.display = "block";
        }
        else if (codigo.trim() == "") {
            msgerror.innerHTML = "Preencha a Descrição";
            document.getElementById("error2").style.display = "none";
            if (msgsucess.style.display == "block")
                msgsucess.style.display = "none";

            msgerror.style.display = "block";
        }
        else if (lote.trim() == "") {
            msgerror.innerHTML = "Preencha o Lote";
            document.getElementById("error2").style.display = "none";
            if (msgsucess.style.display == "block")
                msgsucess.style.display = "none";

            msgerror.style.display = "block";
        }
        else if (saldo.trim() == "") {
            msgerror.innerHTML = "Preencha o saldo";
            document.getElementById("error2").style.display = "none";
            if (msgsucess.style.display == "block")
                msgsucess.style.display = "none";

            msgerror.style.display = "block";
        }
        else {
            var dados = {
                cod_ref,
                codigo,
                lote,
                saldo
            }

            var config = {
                method: "POST",
                headers: {
                    "Content-Type": "application/json; charset=utf-8",
                },
                credentials: 'include', //inclui cookies
                body: JSON.stringify(dados)  //serializa
            };

            fetch("/CadastroProdDist/Criar?id=" + id, config)
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
                            document.getElementById("sucess2").style.display = "none";
                            document.getElementById("error").style.display = "none";
                        }
                        else {
                            msgsucess.innerHTML = "Alteração Realizado!";
                            if (msgerror.style.display == "block")
                                msgerror.style.display = "none";
                            msgsucess.style.display = "block";

                            window.location.href = "/CadastroProdDist/indexListar";
                        }
                        document.getElementById("cod_ref").value = "";
                        document.getElementById("codigo").value = "";
                        document.getElementById("lote").value = "";
                        document.getElementById("saldo").value = "";
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

        fetch("/CadastroProdDist/ObterEditar?id=" + id, config)
            .then(function (dadosJson) {
                var obj = dadosJson.json(); //deserializando
                return obj;
            })
            .then(function (dadosObj) {

                document.getElementById("cod_ref").value = dadosObj.proddist.cod_ref;
                document.getElementById("codigo").value = dadosObj.proddist.cod_prod_dist;
                document.getElementById("lote").value = dadosObj.estoque.lote
                document.getElementById("saldo").value = dadosObj.estoque.saldo
                document.getElementById("saldo").setAttribute("disabled", "disabled");

                index.buscaProdIndustria();

            })
            .catch(function () {

                alert("deu erro")
            })

    },

    buscaProdIndustria: function () {

        var cod_ref = document.getElementById("cod_ref").value;

        var dados = {
            cod_ref
        }

        var config = {
            method: "POST",
            headers: {
                "Content-Type": "application/json; charset=utf-8",
            },
            credentials: 'include', //inclui cookies
            body: JSON.stringify(dados)  //serializa
        };

        fetch("/CadastroProdDist/ObterProd", config)
            .then(function (dadosJson) {
                var obj = dadosJson.json(); //deserializando
                return obj;
            })
            .then(function (dadosObj) {
                document.getElementById("error2").style.display = "none";
                document.getElementById("error2").style.display = "none";
                document.getElementById("sucess2").innerHTML = "Item Encontrado - " + dadosObj.prodemp.descricao;
                document.getElementById("sucess2").style.display = "block";

            })
            .catch(function () {
                document.getElementById("sucess2").style.display = "none";
                document.getElementById("error2").innerHTML = "Item Não Encontrado";
                document.getElementById("error2").style.display = "block";
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