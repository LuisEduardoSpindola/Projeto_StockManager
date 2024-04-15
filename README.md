<body>
    <h1>Documentação StockManager - Luis Eduardo Spindola</h1>
    <h2>Informações do projeto:</h2>
    <p>O projeto foi desenvolvido na versão .NET 8.0 e as dependências utilizadas estão em sua maioria instaladas na versão 8.0.3.</p><br>
    <p>O banco de dados utilizado foi o SQL Server, gerenciado pelo SSMS (SQL Server Management Studio).</p><br>O projeto foi desenvolvido utilizando por base o Domain-Driven Design (DDD), portanto ele foi distribuído em quatro partes:</p>
    <ul>
        <li><strong>Domain:</strong> A camada de domínio na qual foram armazenados os modelos do sistema, sendo eles, Produto, Loja, Itens em estoque e o usuário principal da aplicação, o projeto é uma biblioteca de classes. A camada de domínio não faz referência a nenhuma outra camada, visto que sua responsabilidade no sistema foi apenas armazenar os domínios.</li>
        <li><strong>Application:</strong> A camada de aplicação é responsável por armazenar as interfaces, controllers e a integração com o ViaCep (Mais informações abaixo), o projeto é uma Web API e faz referência a camada de domínio.</li>
        <li><strong>Infrastructure:</strong> A camada de infraestrutura também é uma Web API, entre suas principais responsabilidades estão, armazenar o contexto do banco de dados, armazenar o serviço de geração de token do JWT, armazenar as migrations e os repositories (implementações das interfaces). Essa camada faz referência à camada de domínio e de aplicação.</li>
        <li><strong>Apresentação:</strong> A camada de apresentação é um projeto Web MVC, responsável pela visualização do sistema, ela faz referência a todos os outros projetos.</li>
    </ul>
    <p>O projeto foi desenvolvido por padrão em inglês.</p>
    <h2>RoadMap de desenvolvimento:</h2>
    <p>O processo de desenvolvimento começou com o planejamento do sistema. Por hábito, costumo desenhar o sistema e anotar algumas informações no papel, para clarear os pontos solicitados e pensar em ideias para implementar a demanda de forma eficaz.</p>
    <p>Já no Visual Studio, comecei a montar a arquitetura do projeto, baseado no DDD, instalei as dependências necessárias pelo NuGet e criei o repositório no GitHub para manter o controle sobre o código.</p>
    <p>Não vi necessidade de dois tipos de usuário para esse sistema, então após criar os repositórios e projetos iniciais, comecei desenvolvendo o usuário principal. Além dos atributos já presentes por padrão do IdentityUser, o usuário também tem um nome de usuário mapeado em seu modelo, que também está presente no domínio.</p>
    <p>A configuração JWT foi implementada ao sistema. O gerador de tokens está presente na camada de infraestrutura, e as configurações estão na programação da camada de apresentação.</p>
    <p>Com a parte de Cadastro e Login de usuário funcionando, fiz um primeiro escopo das tabelas, Produto, Loja e Item Estoque. Inicialmente elas foram feitas diretamente pelo SSMS e passadas para o Domain.Models utilizando a engenharia reversa do Entity Framework Core Power Tools, mas apenas em um primeiro momento. Após estabelecer a base das tabelas, as novas propriedades adicionadas aos modelos foram implementadas nos próprios modelos, para que a migration fosse criada e o banco atualizado.</p>
    <p>Já com os modelos prontos na camada de domínio, foram criadas as interfaces para definir e mapear as funcionalidades do sistema. As interfaces estão presentes na camada de aplicação.</p>
    <p>Após criar as interfaces, foram criadas as implementações (Repositories) que herdam e implementam as interfaces. Essas implementações estão presentes na camada de infraestrutura.</p>
    <p>O próximo passo foi o desenvolvimento das controllers, onde está configurado a lógica de cada tela no sistema, entre elas as funcionalidades do CRUD solicitadas e algumas mais específicas, como a busca por nome de produto, por exemplo.</p>
    <h2>Integração com o ViaCep:</h2>
    <p>Com o objetivo de facilitar a experiência do usuário final, foi implementada uma integração com o ViaCep. A partir do CEP informado pelo usuário no cadastro da loja, ao clicar em detalhes, podemos verificar algumas informações do local informado, permitindo que o usuário preencha o cadastro com maior precisão.</p>
</body>
