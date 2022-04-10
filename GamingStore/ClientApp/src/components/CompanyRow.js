import React, { Component } from 'react';

export class CompanyRow extends Component {
    static displayName = CompanyRow.name;

    constructor(props) {
        super(props);

        this.state = {
            token: sessionStorage.getItem('token')
        };
    }

    async deleteCompany(id) {
        await fetch(`../api/companies/${id}`, {
            method: 'DELETE',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${this.state.token}`
            }
        })
            .then(response => {
                if (!response.ok) {
                    return response.text().then(error => { throw new Error(error) });
                }
            })
            .catch(error => alert(error.message));
    }

    updateCompany(id) {
        window.location.href = `/companies/edit/${id}`;
    }

    render() {
        return <tr>
            <td className="text-center">
                <img src={ `data:image/png;base64,${this.props.icon}` } alt="company-icon" width="100" />
            </td>
            <td>{ this.props.id }</td>
            <td>{ this.props.name }</td>
            <td className="text-center d-flex justify-content-around">
                <button onClick={() => this.updateCompany(this.props.id)} className="btn btn-sm btn-info text-white">Edit</button>
                <button onClick={
                    async () => { 
                        await this.deleteCompany(this.props.id); 
                        await this.props.getCompanies(); 
                    }
                } className="btn btn-sm btn-danger">Delete</button>
            </td>
        </tr>;
    }
}