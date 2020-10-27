var indexListar = {

    block: function (id) {
        var acoes = document.getElementById('acoesoption' + id);
        var opcoes = document.getElementsByClassName('dropdown-content')

        for (i = 0; i < opcoes.length; i++) {
            if (opcoes[i].id != "acoesoption" + id)
                opcoes[i].style.display = 'none'
        }

        if (acoes.style.display == 'none')
            acoes.style.display = 'block';
        else
            acoes.style.display = 'none';
    },

    excluir: function (id) {

        var config = {
            method: "DELETE",
            headers: {
                "Content-Type": "application/json; charset=utf-8"
            },
            credentials: 'include', //inclui cookies
        };

        fetch("/CadastroIndustria/Excluir?id=" + id, config)
            .then(function (dadosJson) {
                var obj = dadosJson.json(); //deserializando
                return obj;
            })
            .then(function (dadosObj) {
                if (dadosObj.operacao) {
                    document.getElementById(id).remove()
                }
            })
            .catch(function () {
                alert("Deu erro.")
            })
    },

    alterar: function (id) {
        var config = {
            method: "GET",
            headers: {
                
            },
            credentials: 'include', //inclui cookies
        };

        fetch("/CadastroIndustria/Alterar?id=" + id, config)
            .then(function (dadosJson) {
                var obj = dadosJson.json(); //deserializando
                return obj;
            })
            .then(function (dadosObj) {
                alert(dadosObj.industria.nome);

            })
            .catch(function () {
                alert("Deu erro.")
            })
    },

    btnPesquisarOnClick: function (perfil) {
        document.getElementById("iconsearch").disabled = true;
        var tbodyIndustrias = document.getElementById("table");
        tbodyIndustrias.innerHTML = `
                                   <div class="table-row table-head">
                                        <div class="table-cell first-cell">
                                            <p>Id</p>
                                        </div>
                                        <div class="table-cell">
                                            <p>Nome</p>
                                        </div>
                                        <div class="table-cell">
                                            <p>CNPJ</p>
                                        </div>
                                        <div class="table-cell">
                                            <p>Telefone</p>
                                        </div>
                                        <div class="table-cell last-cell">
                                            <p>Açoes</p>
                                        </div>
                                    </div>
                                    `
        tbodyIndustrias.innerHTML += `
                                    <div class="table-row">
                                        <div class="table-cell first-cell">
                                            <p><img src=\"/img/ajax-loader.gif"\/>carregando...</p>
                                        </div>
                                    </div>
                                    `

        var config = {
            method: "GET",
            headers: {
                "Content-Type": "application/json; charset=utf-8"
            },
            credentials: 'include', //inclui cookies
        };
        var tipo = $(".default_option").text();
        var nome = encodeURIComponent(document.getElementById("industria").value);

        fetch("/CadastroIndustria/Pesquisar?nome=" + nome + "&tipo=" + tipo, config)
            .then(function (dadosJson) {
                var obj = dadosJson.json(); //deserializando
                return obj;
            })
            .then(function (dadosObj) {

                var linhas =  ` 
                        <div class="table-row table-head">
                            <div class="table-cell first-cell">
                                <p>Id</p>
                            </div>
                            <div class="table-cell">
                                <p>Nome</p>
                            </div>
                            <div class="table-cell">
                                <p>CNPJ</p>
                            </div>
                            <div class="table-cell">
                                <p>Telefone</p>
                            </div>
                            <div class="table-cell last-cell">
                                <p>Açoes</p>
                            </div>
                        </div>
                        `;

                for (var i = 0; i < dadosObj.length; i++) {

                    var template =
                        `
                        <div class="table-row" id="${dadosObj[i].id}">
                            <div class="table-cell first-cell">
                                <p>${dadosObj[i].id}</p>
                            </div>
                            <div class="table-cell">
                                <p>${dadosObj[i].nome}</p>
                            </div>
                            <div class="table-cell">
                                <p>${dadosObj[i].cnpj}</p>
                            </div>
                            <div class="table-cell">
                                <p>${dadosObj[i].telefone}</p>
                            </div>
                            <div class="table-cell last-cell">
                                <div class="dropdown" onclick="indexListar.block(${dadosObj[i].id})">
                                    <i class="dropbtn fa fa-fw fa-ellipsis-v"></i>
                                    <div class="dropdown-content" id="acoesoption${dadosObj[i].id}" style="display: none;">
                                        <a data-fancybox data-type="iframe" data-src="/CadastroIndustria/IndexVisualizar?id=${dadosObj[i].id}" href="javascript:;">Visualizar</a>
                                        <a href="/CadastroIndustria/Editar?id=${dadosObj[i].id}">Editar</a>
                                        <a href="javascript:indexListar.excluir(${dadosObj[i].id})" onclick="return confirm('Confirmar Exclusão?')">Excluir</a>
                                  </div>
                                </div>
                            </div>
                        </div>
                        ` 
   
                    linhas += template;
                }

                if (dadosObj.length == 0) {

                    linhas = `
                            <div class="table-row table-head">
                                <div class="table-cell first-cell">
                                    <p>Id</p>
                                </div>
                                <div class="table-cell">
                                    <p>Nome</p>
                                </div>
                                <div class="table-cell">
                                    <p>CNPJ</p>
                                </div>
                                <div class="table-cell">
                                    <p>Telefone</p>
                                </div>
                                <div class="table-cell last-cell">
                                    <p>Açoes</p>
                                </div>
                            </div>
                            <div class="table-row">
                                <div class="table-cell first-cell">
                                        <p>sem resultados.</p>
                                </div>
                            </div>
                            `
                }

                tbodyIndustrias.innerHTML = linhas;
            })
            .catch(function () {
                tbodyIndustrias.innerHTML = `
                                            <div class="table-row">
                                                <div class="table-cell first-cell">
                                                        <p>deu erro.</p>
                                                </div>
                                            </div>
                                            `
            })
            .finally(function () {
                document.getElementById("iconsearch").disabled = false;
            });
        //<td><a href="javascript:indexListar.excluir(${dadosObj[i].id})">Excluir</a></td> 
    }

}