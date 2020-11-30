var index = {
    btnCadastrar: function () {

        var id = document.getElementById("hfIdEditar").value;
        if (id == "")
            id = 0;

        var cod_ref = document.getElementById("cod_ref").value;
        var codigo = document.getElementById("codigo").value;
        var lote = document.getElementById("lote").value;
        var saldo = document.getElementById("saldo").value;

        if (cod_ref.trim() == "") {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Preencha o Código de Referência!'
            })
        }
        else if ($("#sucess").is(":hidden"))
        {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Encontre o Código de referência do Produto!'
            })
        }
        else if (codigo.trim() == "") {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Preencha o Código!'
            })
        }
        else if (lote.trim() == "") {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Preencha o Lote!'
            })
        }
        else if (saldo.trim() == "") {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Preencha o saldo!'
            })
        }
        else {
            document.getElementById('btnCadastrar').classList.add('button-loading')
            document.getElementById("btnCadastrar").disabled = true;

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
                            Swal.fire({
                                position: 'center',
                                icon: 'success',
                                title: 'Cadastro Realizado com Sucesso!',
                                showConfirmButton: false,
                                timer: 1500
                            })
                        }
                        else {
                            Swal.fire({
                                position: 'center',
                                icon: 'success',
                                title: 'Alteração Realizado com Sucesso!',
                                showConfirmButton: false,
                                timer: 1500
                            })
                            document.getElementById("saldo").removeAttribute("disabled");
                            document.getElementById('hfIdEditar').value = "";
                        }
                        document.getElementById("cod_ref").value = "";
                        document.getElementById("codigo").value = "";
                        document.getElementById("lote").value = "";
                        document.getElementById("saldo").value = "";
                        document.getElementById("sucess").style.display = "none";
                    }
                    else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: dadosObj.msg
                        })
                    }

                    document.getElementById('btnCadastrar').classList.remove('button-loading')
                    document.getElementById("btnCadastrar").disabled = "";
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
                if (dadosObj.operacao) {
                    document.getElementById("cod_ref").value = dadosObj.produtoLimpo.cod_ref;
                    document.getElementById("codigo").value = dadosObj.produtoLimpo.codigo;
                    document.getElementById("lote").value = dadosObj.produtoLimpo.lote
                    document.getElementById("saldo").value = dadosObj.produtoLimpo.saldo

                    document.getElementById("saldo").setAttribute("disabled", "disabled");

                    document.getElementById("sucess").innerHTML = "Item Encontrado - " + dadosObj.produtoLimpo.descricao;
                    document.getElementById("sucess").style.display = "block";
                }

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
                document.getElementById("sucess").innerHTML = "Item Encontrado - " + dadosObj.prodemp.descricao;
                document.getElementById("sucess").style.display = "block";
            })
            .catch(function () {
                document.getElementById("sucess").innerHTML = "";
                document.getElementById("sucess").style.display = "none";
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: 'Item Não Encontrado!'
                })
            })
    },
    buscaProdDist: function () {

        var codigo = document.getElementById("codigo").value;

        var dados = {
            codigo
        }

        var config = {
            method: "POST",
            headers: {
                "Content-Type": "application/json; charset=utf-8",
            },
            credentials: 'include', //inclui cookies
            body: JSON.stringify(dados)  //serializa
        };

        fetch("/CadastroProdDist/ObterProdDist", config)
            .then(function (dadosJson) {
                var obj = dadosJson.json(); //deserializando
                return obj;
            })
            .then(function (dadosObj) {
                if (dadosObj.operacao) {
                    document.getElementById('cod_ref').value = dadosObj.cod_ref
                    document.getElementById("sucess").innerHTML = "Item Encontrado - " + dadosObj.descricao;
                    document.getElementById("sucess").style.display = "block";
                }
                else {
                    document.getElementById("sucess").innerHTML = "";
                    document.getElementById("sucess").style.display = "none";
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'Item Não Encontrado!'
                    })
                }   

            })
            .catch(function () {
                document.getElementById("sucess").innerHTML = "";
                document.getElementById("sucess").style.display = "none";
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: 'Item Não Encontrado!'
                })
            })
    }
}

if (document.getElementById("hfIdEditar") != null) {
    if (document.getElementById("hfIdEditar").value != "") {
        index.obterDadosEditar(document.getElementById("hfIdEditar").value)
    }
}

document.getElementById("cod_ref")
    .addEventListener("keyup", function (event) {
        event.preventDefault();
        if (event.keyCode === 13) {
            index.buscaProdIndustria();
        }
    });

document.getElementById("codigo")
    .addEventListener("keyup", function (event) {
        event.preventDefault();
        if (event.keyCode === 13) {
            index.buscaProdDist();
        }
    });