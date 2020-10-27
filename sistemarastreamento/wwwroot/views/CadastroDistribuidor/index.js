var linhaCidade = 1;
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

    btnCadastrar: function () {

        var id = document.getElementById("hfIdEditar").value;
        if (id == "")
            id = 0;
        var cnpj = document.getElementById("cnpj").value;
        var nome = document.getElementById("nome").value;
        var ie = document.getElementById("ie").value;
        var representante = document.getElementById("representante").value;
        var rua = document.getElementById("rua").value;
        var numero = document.getElementById("numero").value;
        var bairro = document.getElementById("bairro").value;
        var telefone = document.getElementById("telefone").value;
        var email = document.getElementById("email").value;
        var senha = document.getElementById("senha").value;
        var senhaConf = document.getElementById("senhaConf").value;
        var estado = document.getElementById("selUF").value;
        var cidade = document.getElementById("selCidade").value;
        var perfilId = document.getElementById("hfperfilId").value;

        var msgerro = document.getElementById("error");
        var msgsucess = document.getElementById("sucess");

        msgerro.innerHTML = "";
        msgerro.style.display = "none";

        msgsucess.innerHTML = "";
        msgsucess.style.display = "none";

        var checkemail = index.validacaoEmail(email);

        if (cnpj.trim() == "") {
            msgerro.innerHTML = "Preencha o CNPJ";
            msgerro.style.display = "block";
        }
        else if (cnpj.length < 18) {
            msgerro.innerHTML = "Preencha todos os numéros do CNPJ";
            msgerro.style.display = "block";
        }
        else if (ie.trim() == "") {
            msgerro.innerHTML = "Preencha a Inscrição Estadual </br>";
            msgerro.style.display = "block";
        }
        else if (ie.length < 13) {
            msgerro.innerHTML = "Preencha todos os numéros da Inscrição Estadual";
            msgerro.style.display = "block";
        }
        else if (nome.trim() == "") {
            msgerro.innerHTML = "Preencha o Nome </br>";
            msgerro.style.display = "block";
        }
        else if (representante.trim() == "") {
            msgerro.innerHTML = "Preencha o representante </br>";
            msgerro.style.display = "block";
        }
        else if (telefone.trim() == "") {
            msgerro.innerHTML = "Preencha o telefone </br>";
            msgerro.style.display = "block";
        }
        else if (selUF.selectedIndex == 0) {
            msgerro.innerHTML = "Preencha a UF </br>";
            msgerro.style.display = "block";
        }
        else if (selCidade.selectedIndex == 0) {
            msgerro.innerHTML = "Preencha a Cidade </br>";
            msgerro.style.display = "block";
        }
        else if (rua.trim() == "") {
            msgerro.innerHTML = "Preencha a rua </br>";
            msgerro.style.display = "block";
        }
        else if (numero.trim() == "") {
            msgerro.innerHTML = "Preencha o número </br>";
            msgerro.style.display = "block";
        }
        else if (bairro.trim() == "") {
            msgerro.innerHTML = "Preencha o bairro </br>";
            msgerro.style.display = "block";
        }
        else if (email.trim() == "") {
            msgerro.innerHTML = "Preencha o E-mail </br>";
            msgerro.style.display = "block";
        }
        else if (!checkemail) {
            msgerro.innerHTML = "E-mail inválido </br>";
            msgerro.style.display = "block";
        }
        else if (perfilId == 0) {
            msgerro.innerHTML = "Preencha o perfil </br>";
            msgerro.style.display = "block";
        }
        else if (senha.trim() == "") {
            msgerro.innerHTML = "Preencha a senha </br>";
            msgerro.style.display = "block";
        }
        else if (senhaConf.trim() == "") {
            msgerro.innerHTML = "Confirme a senha </br>";
            msgerro.style.display = "block";
        }
        else if (senha.trim() != senhaConf.trim()) {
            msgerro.innerHTML = "Senhas diferentes </br>";
            msgerro.style.display = "block";
        }
        else {
            var dados = {
                cnpj,
                nome,
                ie,
                representante,
                rua,
                numero,
                bairro,
                telefone,
                email,
                senha,
                senhaConf,
                estado,
                cidade
            }

            var config = {
                method: "POST",
                headers: {
                    "Content-Type": "application/json; charset=utf-8",
                },
                credentials: 'include', //inclui cookies
                body: JSON.stringify(dados)  //serializa
            };

            fetch("/CadastroDistribuidor/Criar?id=" + id, config)
                .then(function (dadosJson) {
                    var obj = dadosJson.json(); //deserializando
                    return obj;
                })
                .then(function (dadosObj) {
                    if (dadosObj.operacao && id == 0) {

                        var email = dadosObj.email;
                        var senha = dadosObj.senha;
                        var tipo = "Distribuidor";

                        var dados2 = {
                            email,
                            senha,
                            tipo,
                            perfilId
                        }

                        var config2 = {
                            method: "POST",
                            headers: {
                                "Content-Type": "application/json; charset=utf-8",
                            },
                            credentials: 'include', //inclui cookies
                            body: JSON.stringify(dados2)  //serializa
                        };

                        fetch("/Login/Criar", config2)
                            .then(function (dadosJson) {
                                var obj = dadosJson.json(); //deserializando
                                return obj;
                            })
                            .then(function (dadosObj) {
                                if (dadosObj.operacao) {
                                    msgsucess.innerHTML = "Cadastro Realizado!";
                                    msgsucess.style.display = "block";
                                }

                                window.location.href = "/Login";
                            })
                            .catch(function (e) {
                                alert("deu erro");
                            })
                    }
                    else if (id > 0) {
                        msgsucess.innerHTML = "Alteração Realizado!";
                        msgsucess.style.display = "block";

                        window.location.href = "/CadastroDistribuidor/indexListar";
                    }
  
                })
                .catch(function (e) {
                    alert("deu erro");
                })
        }
    },

    buscarEstados: function () {

        var config = {
            method: "GET",
            headers: {
                "Content-Type": "application/json; charset=utf-8"
            },
            credentials: 'include', //inclui cookies
        };

        fetch("/Cidade/ObterEstados", config)
            .then(function (dadosJson) {
                var obj = dadosJson.json(); //deserializando
                return obj;
            })
            .then(function (dadosObj) {

                var selUF = document.getElementById("selUF");
                var opts = "<option value=''></option>";
                for (var i = 0; i < dadosObj.length; i++) {

                    opts += `<option 
                            value="${dadosObj[i].id}">
                            ${dadosObj[i].uf}</option>`;
                    //opts += "<option value='" + dadosObj[i] + "'>" + dadosObj[i] + "</option>"
                }

                selUF.innerHTML = opts;
                if (selUF.selectedIndex == 0) {
                    selUF.selectedIndex = 26;
                    index.buscarCidades(26);
                }


            })
            .catch(function () {
                alert("Algo deu Errado, Tente Novamente!")
            })

    },

    buscarCidades: function (uf) {

        var config = {
            method: "GET",
            headers: {
                "Content-Type": "application/json; charset=utf-8"
            },
            credentials: 'include', //inclui 
        };

        fetch("/Cidade/ObterCidades?uf=" + uf, config)
            .then(function (dadosJson) {
                var obj = dadosJson.json(); //deserializando
                return obj;
            })
            .then(function (dadosObj) {

                var selCidade = document.getElementById("selCidade");
                var opts = "<option value=''></option>";
                for (var i = 0; i < dadosObj.length; i++) {

                    opts += `<option
                        value="${dadosObj[i].id}">
                        ${dadosObj[i].nome}</option>`;
                }

                selCidade.innerHTML = opts;
                if (linhaCidade == 1)
                    selCidade.selectedIndex = 1;
                else
                    selCidade.value = linhaCidade;

            })
            .catch(function () {
                alert("Algo deu Errado, Tente Novamente!")
            })
        
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

        fetch("/CadastroDistribuidor/ObterEditar?id=" + id, config)
            .then(function (dadosJson) {
                var obj = dadosJson.json(); //deserializando
                return obj;
            })
            .then(function (dadosObj) {

                document.getElementById("cnpj").value = dadosObj.distribuidor.cnpj;
                document.getElementById("nome").value = dadosObj.distribuidor.nome;
                document.getElementById("ie").value = dadosObj.distribuidor.ie;
                document.getElementById("representante").value = dadosObj.distribuidor.representante;
                document.getElementById("selUF").selectedIndex = dadosObj.distribuidor.estado;
                linhaCidade = dadosObj.distribuidor.cidade;
                index.buscarCidades(dadosObj.distribuidor.estado);
                document.getElementById("rua").value = dadosObj.distribuidor.rua;
                document.getElementById("numero").value = dadosObj.distribuidor.numero;
                document.getElementById("bairro").value = dadosObj.distribuidor.bairro;
                document.getElementById("telefone").value = dadosObj.distribuidor.telefone;
                document.getElementById("email").value = dadosObj.distribuidor.email;
                document.getElementById("hfperfilId").value = dadosObj.perfil.id;
                document.getElementById("perfilNome").value = dadosObj.perfil.nome;
                document.getElementById("senha").value = dadosObj.distribuidor.senha;
                document.getElementById("senhaConf").value = dadosObj.distribuidor.senha;
                document.getElementById("email").disabled = "true";
                document.getElementById("senha").disabled = "true";
                document.getElementById("senhaConf").disabled = "true";
            })
            .catch(function () {

                alert("deu erro")
            })

    },

    selecionarPerfil: function (id, nome) {
        document.getElementById("hfperfilId").value = id;
        document.getElementById("perfilNome").value = nome;
        $.fancybox.close();
    }
}

//iniciando a página;
index.buscarEstados();

if (document.getElementById("hfIdEditar") != null) {
    if (document.getElementById("hfIdEditar").value != "") {

        index.obterDadosEditar(document.getElementById("hfIdEditar").value)

    }
}