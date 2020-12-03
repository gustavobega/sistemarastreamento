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

        var checkemail = index.validacaoEmail(email);

        if (cnpj.trim() == "") {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Preencha o CNPJ!'
            })
        }
        else if (cnpj.length < 18) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Preencha todos os numéros do CNPJ!'
            })
        }
        else if (ie.trim() == "") {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Preencha a Inscrição Estadual!'
            })
        }
        else if (ie.length < 13) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Preencha todos os numéros da Inscrição Estadual!'
            })
        }
        else if (nome.trim() == "") {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Preencha o Nome!'
            })
        }
        else if (representante.trim() == "") {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Preencha o representante!'
            })
        }
        else if (telefone.trim() == "") {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Preencha o telefone!'
            })
        }
        else if (selUF.selectedIndex == 0) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Preencha a UF!'
            })
        }
        else if (selCidade.selectedIndex == 0) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Preencha a Cidade!'
            })
        }
        else if (rua.trim() == "") {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Preencha a rua!'
            })
        }
        else if (numero.trim() == "") {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Preencha o número!'
            })
        }
        else if (bairro.trim() == "") {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Preencha o bairro!'
            })
        }
        else if (email.trim() == "") {
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
                text: 'E-mail inválido!'
            })
        }
        else if (perfilId == 0) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Preencha o perfil!'
            })
        }
        else if (senha.trim() == "") {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Preencha a senha!'
            })
        }
        else if (senha.trim().length < 6) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Informe uma senha mais forte!'
            })
        }
        else if (senhaConf.trim() == "") {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Confirme a senha!'
            })
        }
        else if (senha.trim() != senhaConf.trim()) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Senhas diferentes!'
            })
        }
        else {

            document.getElementById('btnCadastrar').classList.add('button-loading')
            document.getElementById("btnCadastrar").disabled = true;

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
                cidade,  
            }

            var config = {
                method: "POST",
                headers: {
                    "Content-Type": "application/json; charset=utf-8",
                },
                credentials: 'include', //inclui cookies
                body: JSON.stringify(dados)  //serializa
            };

            fetch("/CadastroIndustria/Criar?id=" + id, config)
                .then(function (dadosJson) {
                    var obj = dadosJson.json(); //deserializando
                    return obj;
                })
                .then(function (dadosObj) {
                    if (dadosObj.operacao && id == 0) {

                        var email = dadosObj.email;
                        var senha = dadosObj.senha;
                        var tipo = "Indústria";

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
                                    Swal.fire({
                                        position: 'center',
                                        icon: 'success',
                                        title: 'Cadastro Realizado com Sucesso!',
                                        showConfirmButton: false,
                                        timer: 1500
                                    })
                                }
                            })
                            .catch(function (e) {
                                alert("deu erro");
                            })
                    }
                    else if (id > 0) {

                        var email2 = document.getElementById("email").value;
                        var dados3 = {
                            email2,
                            perfilId
                        }

                        var config3 = {
                            method: "POST",
                            headers: {
                                "Content-Type": "application/json; charset=utf-8",
                            },
                            credentials: 'include', //inclui cookies
                            body: JSON.stringify(dados3)  //serializa
                        };

                        fetch("/Login/AlterarPerfil", config3)
                            .then(function (dadosJson) {
                                var obj = dadosJson.json(); //deserializando
                                return obj;
                            })
                            .then(function (dadosObj) {
                                if (dadosObj.operacao) {
                                    index.limpacampos();
                                    Swal.fire({
                                        position: 'center',
                                        icon: 'success',
                                        title: 'Alteração Realizado com Sucesso!',
                                        showConfirmButton: false,
                                        timer: 1500
                                    })
                                }
                            })
                            .catch(function (e) {
                                alert("deu erro");
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

        fetch("/CadastroIndustria/ObterEditar?id=" + id, config)
            .then(function (dadosJson) {
                var obj = dadosJson.json(); //deserializando
                return obj;
            })
            .then(function (dadosObj) {

                document.getElementById("cnpj").value = dadosObj.industria.cnpj;
                document.getElementById("nome").value = dadosObj.industria.nome;
                document.getElementById("ie").value = dadosObj.industria.ie;
                document.getElementById("representante").value = dadosObj.industria.representante;
                document.getElementById("selUF").value = dadosObj.industria.estado;
                linhaCidade = dadosObj.industria.cidade;
                index.buscarCidades(dadosObj.industria.estado);
                document.getElementById("rua").value = dadosObj.industria.rua;
                document.getElementById("numero").value = dadosObj.industria.numero;
                document.getElementById("bairro").value = dadosObj.industria.bairro;
                document.getElementById("telefone").value = dadosObj.industria.telefone;            
                document.getElementById("email").value = dadosObj.industria.email;
                document.getElementById("hfperfilId").value = dadosObj.perfil.id;
                document.getElementById("perfilNome").value = dadosObj.perfil.nome;
                document.getElementById("senha").value = dadosObj.industria.senha;
                document.getElementById("senhaConf").value = dadosObj.industria.senha;
                document.getElementById("email").disabled = "true";
                document.getElementById("senha").disabled = "true";
                document.getElementById("senhaConf").disabled = "true";
                
            })
            .catch(function () {
                alert("Algo deu Errado, Tente Novamente!")
            })

    },

    selecionarPerfil: function (id, nome) {
        document.getElementById("hfperfilId").value = id;
        document.getElementById("perfilNome").value = nome;
        $.fancybox.close();
    },
    limpacampos: function () {
        document.getElementById("cnpj").value = "";
        document.getElementById("nome").value = "";
        document.getElementById("ie").value = "";
        document.getElementById("representante").value = "";
        document.getElementById("rua").value = "";
        document.getElementById("numero").value = "";
        document.getElementById("bairro").value = "";
        document.getElementById("telefone").value = "";
        document.getElementById("email").value = "";
        document.getElementById("senha").value = "";
        document.getElementById("senhaConf").value = "";
        document.getElementById("perfilNome").value = "";
        document.getElementById("selUF").selectedIndex = 26;
        index.buscarCidades(26);
    }
}

//iniciando a página;
index.buscarEstados();

if (document.getElementById("hfIdEditar") != null) {
    if (document.getElementById("hfIdEditar").value != "") {
        index.obterDadosEditar(document.getElementById("hfIdEditar").value)
    }    
}