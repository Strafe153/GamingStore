import React, { Component } from 'react';
import { CompanyRow } from './CompanyRow';

export class Companies extends Component {
    static displayName = Companies.name;

    constructor(props) {
        super(props);

        this.state = {
            companies: []
        };

        this.getCompanies = this.getCompanies.bind(this);
    }

    async componentDidMount() {
        await this.getCompanies();
    }

    async getCompanies() {
        await fetch('../api/companies', {
            method: 'GET'
        })
            .then(response => {
                if (response.ok) {
                    return response.json();
                }

                return response.text().then(error => { throw new Error(error) });
            })
            .then(data => this.setState({companies: data}))
            .catch(error => alert(error.message));
    }

    handleCompanies = companies => {
        this.setState({companies: companies});
    }

    render() {
        return <div>
            <table className="table table-bordered">
                <thead>
                    <tr>
                        <th className="text-center">Id</th>
                        <th className="text-center">Name</th>
                        <th className="text-center">Actions</th>
                    </tr>
                </thead>
                <tbody>
                {
                    this.state.companies.map(company => {
                        return <CompanyRow onGetCompanies={this.handleCompanies}
                                        key={company.id}
                                        id={company.id}
                                        name={company.name}
                                        getCompanies={this.getCompanies} />
                    })
                }
                </tbody>
            </table>

            <a className="btn btn-primary" href="/companies/add">Add</a>
        </div>;
    }
}