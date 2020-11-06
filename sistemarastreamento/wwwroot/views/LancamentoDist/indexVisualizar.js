var index = {

    obterInfoNota: function () {

        id_nota = document.getElementById("hfIdNota").value;
        var tbodyItens = document.getElementById("table");

        document.getElementById("warning").innerHTML = "Caso os dados do destino não estiverem certo clique aqui em: <a href=" + "javascript:index.editarcampos()" + ">Editar</a>";
        document.getElementById("warning").style.display = 'block';

        //é apenas para visualizar /LancamentoDist length == 2 ou /LancamentoDist/index.js length == 3
        var local = window.parent.location.pathname;
        var str = local.split("/");
        if (str.length == 3)
            document.getElementById("warning").style.display = 'none';

        var config = {
            method: "GET",
            headers: {
                "Content-Type": "application/json; charset=utf-8"
            },
            credentials: 'include', //inclui cookies
        };

        fetch("/LancamentoDist/carregaItensNota?id=" + id_nota, config)

            .then(function (dadosJson) {
                var obj = dadosJson.json(); //deserializando
                return obj;
            })
            .then(function (dadosObj) {

                var linhas = "";
                var obj = JSON.parse(dadosObj);
                for (var i = 0; i < obj.length; i++) {

                    var template =
                        `
                        <div class="table-row">
                            <div class="table-cell first-cell">
                                <p>${obj[i].cod_prod_dist}</p>
                            </div>
                            <div class="table-cell">
                                <p>${obj[i].descricao}</p>
                            </div>
                            <div class="table-cell">
                                <p>${obj[i].lote}</p>
                            </div>
                            <div class="table-cell">
                                <p>${obj[i].qtde}</p>
                            </div>

                            <div class="table-cell last-cell">
                                <p>R$${obj[i].valor_unit}</p>
                            </div>

                        </div>
                        `

                    linhas += template;

                }
                tbodyItens.innerHTML += linhas;

            })
            .catch(function () {
                alert("Deu erro.")
            })
    },

    editarcampos: function () {

        var dados = window.parent.document.getElementById("info").innerHTML;
        document.getElementById("info").innerHTML = dados;
        document.getElementById("info").style.display = 'block';
        document.getElementById("botaosalvar").style.display = 'block';
        document.getElementById("btnCancelar").style.display = 'block';

        document.getElementById("paciente").removeAttribute("disabled");
        document.getElementById("data_cirurgia").removeAttribute("disabled");
        document.getElementById("medico").removeAttribute("disabled");
        document.getElementById("convenio").removeAttribute("disabled");
        document.getElementById("hospital").removeAttribute("disabled");

        document.getElementById("warning").style.display = 'none';
    },

    btnSalvar: function () {

        id = document.getElementById("hfIdNota").value;

        paciente = document.getElementById("paciente").value;
        data_cirurgia = document.getElementById("data_cirurgia").value;
        medico = document.getElementById("medico").value;
        convenio = document.getElementById("convenio").value;
        hospital = document.getElementById("hospital").value;

        msgerror = document.getElementById("error");

        if (paciente.trim() == "") {
            msgerror.innerHTML = "Preencha o Paciente";
            msgerror.style.display = "block";
        }
        else if (data_cirurgia.trim() == "") {
            msgerror.innerHTML = "Preencha a Data";
            msgerror.style.display = "block";
        }
        else if (medico.trim() == "") {
            msgerror.innerHTML = "Preencha o Médico";
            msgerror.style.display = "block";
        }
        else if (convenio.trim() == "") {
            msgerror.innerHTML = "Preencha o Convênio";
            msgerror.style.display = "block";
        }
        else if (hospital.trim() == "") {
            msgerror.innerHTML = "Preencha o Hospital";
            msgerror.style.display = "block";
        }
        else {

            var dados = {
                paciente,
                data_cirurgia,
                medico,
                convenio,
                hospital
            }

            var config = {
                method: "POST",
                headers: {
                    "Content-Type": "application/json; charset=utf-8",
                },
                credentials: 'include', //inclui cookies
                body: JSON.stringify(dados)  //serializa
            };

            fetch("/LancamentoDist/Alterar?id= " + id, config)
                .then(function (dadosJson) {
                    var obj = dadosJson.json();
                    return obj;
                })
                .then(function (dadosObj) {

                    document.getElementById("warning").style.display = 'block';
                    document.getElementById("info").style.display = 'none';

                    document.getElementById("paciente").disabled = true;
                    document.getElementById("data_cirurgia").disabled = true;
                    document.getElementById("medico").disabled = true;
                    document.getElementById("convenio").disabled = true;
                    document.getElementById("hospital").disabled = true;

                    document.getElementById("botaosalvar").style.display = 'none';
                    document.getElementById("btnCancelar").style.display = 'none';
                    document.getElementById("error").style.display = 'none';

                })
                .catch(function (e) {
                    alert("deu erro");
                })  

        }
 
    },

    btnCancelar: function () {

        paciente = document.getElementById("paciente").value;
        data_cirurgia = document.getElementById("data_cirurgia").value;
        medico = document.getElementById("medico").value;
        convenio = document.getElementById("convenio").value;
        hospital = document.getElementById("hospital").value;

        msgerror = document.getElementById('error');

        if (paciente.trim() == "") {
            msgerror.innerHTML = "Não deixe campo(s) vázio(s)!";
            msgerror.style.display = "block";
        }
        else if (data_cirurgia.trim() == "") {
            msgerror.innerHTML = "Não deixe campo(s) vázio(s)!";
            msgerror.style.display = "block";
        }
        else if (medico.trim() == "") {
            msgerror.innerHTML = "Não deixe campo(s) vázio(s)!";
            msgerror.style.display = "block";
        }
        else if (convenio.trim() == "") {
            msgerror.innerHTML = "Não deixe campo(s) vázio(s)!";
            msgerror.style.display = "block";
        }
        else if (hospital.trim() == "") {
            msgerror.innerHTML = "Não deixe campo(s) vázio(s)!";
            msgerror.style.display = "block";
        }
        else {
            document.getElementById("warning").style.display = 'block';
            document.getElementById("info").style.display = 'none';

            document.getElementById("paciente").disabled = true;
            document.getElementById("data_cirurgia").disabled = true;
            document.getElementById("medico").disabled = true;
            document.getElementById("convenio").disabled = true;
            document.getElementById("hospital").disabled = true;

            document.getElementById("botaosalvar").style.display = 'none';
            document.getElementById("btnCancelar").style.display = 'none';
            document.getElementById("error").style.display = 'none';
        }
  
    }
}
//para chamar a função JS logo apos o carregamento da página
index.obterInfoNota();
