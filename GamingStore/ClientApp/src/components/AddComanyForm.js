import React, { Component } from 'react';

export class AddCompanyForm extends Component {
    static displayName = AddCompanyForm.name;

    constructor(props) {
        super(props);

        this.state = {
            token: sessionStorage.getItem('token'),
            name: ''
        };

        this.addCompany = this.addCompany.bind(this);
        this.handleName = this.handleName.bind(this);
    }

    async addCompany(event) {
        event.preventDefault();

        await fetch('../api/companies', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${this.state.token}`
            },
            body: JSON.stringify(this.state)
        })
            .then(response => {
                if (response.ok) {
                    return response.json();
                }

                return response.text().then(error => { throw new Error(error) });
            })
            .then(() => window.location.href = '/companies')
            .catch(error => alert(error.message));
    }

    handleName(event) {
        this.setState({name: event.target.value});
    }

    render() {
        return <form onSubmit={this.addCompany}>
            <div className='form-group mb-3'>
                <label htmlFor='name' className='form-label'>Name:</label>
                <input id='name' className='form-control' type='text' value={this.state.name} onChange={this.handleName} />
            </div>

            <input className='btn btn-primary' type='submit' value='Add' />
        </form>;
    }
}