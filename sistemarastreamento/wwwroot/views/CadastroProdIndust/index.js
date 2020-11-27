var index = {
    btnCadastrar: function () {
        
        var id = document.getElementById("hfIdEditar").value;
        if (id == "")
            id = 0;

        var cod_ref = document.getElementById("cod_ref").value;
        var descricao = document.getElementById("descricao").value;

        if (cod_ref.trim() == "") {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Preencha o Código de Referência!'
            })
        }
        else if (descricao.trim() == "") {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Preencha a Descrição!'
            })
        }
        else {
            document.getElementById('btnCadastrar').value = 'aguarde...'
            document.getElementById('btnCadastrar').style.background = '#5092c8'
            document.getElementById('gif-login').style.display = 'block'

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
                        }
                        document.getElementById("cod_ref").value = "";
                        document.getElementById("descricao").value = "";
                    }
                    else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: 'Código já Existente!'
                        })
                    }
                    document.getElementById('btnCadastrar').value = 'Cadastrar'
                    document.getElementById('gif-login').style.display = 'none'
                    document.getElementById('btnCadastrar').style.background = '#4682B4'
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

    }
}

if (document.getElementById("hfIdEditar") != null) {
    if (document.getElementById("hfIdEditar").value != "") {
        index.obterDadosEditar(document.getElementById("hfIdEditar").value)
    }
}