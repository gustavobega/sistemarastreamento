var index = {

    btnImportar: function () {

        document.getElementById("error").style.display = 'none';

        var carrega = document.getElementById("carrega");
        
        var file = document.getElementById("xml").files[0];

        var fd = new FormData();
        fd.append("fileXML", file);

        if (document.getElementById("xml").files.length == 0) {
            document.getElementById("error").innerHTML = "Selecione um Arquivo";
            document.getElementById("error").style.display = "block";
        }
        else {
            carrega.innerHTML = `<tr><td colspan="3"><img src=\"/img/ajax-loader.gif"\ /> verificando...</td></tr>`
            var configFD = {
                method: "POST",
                headers: {
                    "Accept": "application/json",
                },
                body: fd
            };

            fetch("/LancamentoIndust/ImportXML", configFD)
                .then(function (dadosJson) {
                    var obj = dadosJson.json(); //deserializando
                    return obj;
                })
                .then(function (dadosObj) {
                    if (dadosObj.operacao) {    
                        document.getElementById("error").style.display = "none";
                        document.getElementById("sucess").innerHTML = dadosObj.msg;
                        document.getElementById("sucess").style.display = "block";

                        //cadastro/altera no estoque também
                        var cnpjdist = dadosObj.cnpjdist;
                        var id_nota = dadosObj.id_nota;

                        var dados = {
                            cnpjdist   
                        }

                        var config2 = {
                            method: "POST",
                            headers: {
                                "Content-Type": "application/json; charset=utf-8",
                            },
                            credentials: 'include', //inclui cookies
                            body: JSON.stringify(dados)  //serializa
                        };

                        fetch("/LancamentoIndust/CriarEstoque?id_nota=" + id_nota, config2)
                            .then(function (dadosJson) {
                                var obj = dadosJson.json();
                                return obj;
                            })
                            .then(function (dadosObj) {

                            })
                    }
                    else {
                        document.getElementById("sucess").style.display = "none";
                        document.getElementById("error").innerHTML = dadosObj.msg;
                        document.getElementById("error").style.display = "block";
                    }
                    
                })
                .catch(function () {
                    document.getElementById("sucess").style.display = "none";
                    document.getElementById("error").innerHTML = "Algo deu Errado, Tente Novamente! </br>";
                    document.getElementById("error").style.display = "block";
                })
                .finally(function () {
                    carrega.innerHTML = "";
                });

        }
        
    }

}